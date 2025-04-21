using AutoMapper;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Commands.AddFavorite
{
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, string>
    {
        private readonly IGenericRepository<Domain.Models.Favorite> _favoriteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddFavoriteCommandHandler> _logger;

        public AddFavoriteCommandHandler(
            IGenericRepository<Domain.Models.Favorite> favoriteRepository,
            IMapper mapper,
            ILogger<AddFavoriteCommandHandler> logger)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding favorite for UserId: {UserId}, RecipeId: {RecipeId}", request.UserId, request.RecipeId);

            var exists = await _favoriteRepository.ExistsAsync(f => f.UserId == request.UserId && f.RecipeId == request.RecipeId);
            if (exists)
            {
                _logger.LogWarning("Favorite already exists for UserId: {UserId}, RecipeId: {RecipeId}", request.UserId, request.RecipeId);
                return "Already added to favorites.";
            }

            var favorite = _mapper.Map<Domain.Models.Favorite>(request);
            await _favoriteRepository.AddAsync(favorite);

            _logger.LogInformation("Favorite added successfully.");
            return "Added to favorites successfully.";
        }
    }



}
