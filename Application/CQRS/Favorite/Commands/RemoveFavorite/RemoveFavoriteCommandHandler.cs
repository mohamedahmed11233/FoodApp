using Infrastructure.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, string>
    {
        private readonly IGenericRepository<Domain.Models.Favorite> _favoriteRepository;
        private readonly ILogger<RemoveFavoriteCommandHandler> _logger;

        public RemoveFavoriteCommandHandler(
            IGenericRepository<Domain.Models.Favorite> favoriteRepository,
            ILogger<RemoveFavoriteCommandHandler> logger)
        {
            _favoriteRepository = favoriteRepository;
            _logger = logger;
        }

        public async Task<string> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to remove favorite for UserId: {UserId}, RecipeId: {RecipeId}", request.UserId, request.RecipeId);

            var allFavorites = await _favoriteRepository.GetAllAsync();
            var favorite = allFavorites.FirstOrDefault(f => f.UserId == request.UserId && f.RecipeId == request.RecipeId);

            if (favorite == null)
            {
                _logger.LogWarning("Favorite not found.");
                return "Favorite not found.";
            }

            await _favoriteRepository.DeleteAsync(favorite);
            _logger.LogInformation("Favorite removed successfully.");

            return "Removed from favorites.";
        }
    }


}
