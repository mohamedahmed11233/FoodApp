using Application.Dtos.Favorite;
using AutoMapper;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Queries
{
    public class GetFavoritesByUserIdQueryHandler : IRequestHandler<GetFavoritesByUserIdQuery, List<FavoriteDto>>
    {
        private readonly ILogger<GetFavoritesByUserIdQueryHandler> _logger;
        private IGenericRepository<Domain.Models.Favorite> _favoriteRepository;
        private readonly IMapper _mapper;

        public GetFavoritesByUserIdQueryHandler(
            IGenericRepository<Domain.Models.Favorite> favoriteRepository,
            IMapper mapper,
            ILogger<GetFavoritesByUserIdQueryHandler> logger)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<FavoriteDto>> Handle(GetFavoritesByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching favorites for UserId: {UserId}", request.UserId);

            var allFavorites = await _favoriteRepository.GetAllAsync();
            var userFavorites = allFavorites.Where(f => f.UserId == request.UserId);
            var result = _mapper.Map<List<FavoriteDto>>(userFavorites);

            _logger.LogInformation("Fetched {Count} favorites for UserId: {UserId}", result.Count, request.UserId);

            return result;
        }
    }

    }
