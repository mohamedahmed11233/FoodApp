using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;

namespace Application.CQRS.Auth.Queries.AssignFeatures
{
    public class CheckRoleHasFeatureQueryHandler : IRequestHandler<CheckRoleHasFeatureQuery, bool>
    {
        private readonly IGenericRepository<RoleFeature> _repository;

        public CheckRoleHasFeatureQueryHandler(IGenericRepository<RoleFeature> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CheckRoleHasFeatureQuery request, CancellationToken cancellationToken)
        {
            var features = await _repository.GetAllWithSpecAsync(rf => rf.Role == request.Role && rf.Feature == request.Feature);
            return features != null && features.Any();
        }
    }
}
