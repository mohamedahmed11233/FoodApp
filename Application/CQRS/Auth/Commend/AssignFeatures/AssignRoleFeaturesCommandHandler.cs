using Application.CQRS.Auth.Commend.AssignFeatures;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

public class AssignRoleFeaturesCommandHandler : IRequestHandler<AssignRoleFeaturesCommand, bool>
{
    private readonly IGenericRepository<RoleFeature> _repository;
    private readonly ILogger<AssignRoleFeaturesCommandHandler> _logger;

    public AssignRoleFeaturesCommandHandler(IGenericRepository<RoleFeature> repository,ILogger<AssignRoleFeaturesCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(AssignRoleFeaturesCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            _logger.LogWarning("AssignRoleFeaturesCommand is null.");
            return false;
        }

        try
        {
            var oldFeatures = await _repository.GetAllWithSpecAsync(rf => rf.Role == request.Role);

            if (oldFeatures != null && oldFeatures.Any())
                await _repository.DeleteRangeAsync(oldFeatures); // Bulk delete

            if (request.Features != null && request.Features.Any())
            {
                var uniqueFeatures = request.Features.Distinct().ToList();

                var newFeatures = uniqueFeatures.Select(feature => new RoleFeature
                {
                    Role = request.Role,
                    Feature = feature
                }).ToList();

                await _repository.AddRangeAsync(newFeatures); // Bulk insert
            }

            await _repository.SaveChangesAsync();

            _logger.LogInformation("Successfully assigned {Count} features to Role {Role}",
                request.Features?.Count ?? 0, request.Role);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign features to Role {Role}", request.Role);
            return false;
        }
    }
}
