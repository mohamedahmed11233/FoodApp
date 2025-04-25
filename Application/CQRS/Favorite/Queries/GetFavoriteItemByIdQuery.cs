
using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Queries
{
    public sealed record GetFavoriteItemByIdQuery(int Id) : IRequest<RecipeDto>;
    public class GetFavoriteItemByIdQueryHandler : IRequestHandler<GetFavoriteItemByIdQuery, RecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFavoriteItemByIdQueryHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<RecipeDto> Handle(GetFavoriteItemByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var Recipe = await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync( request.Id);
            if (Recipe.IsFavorite is true) 
                return _mapper.Map<RecipeDto>(Recipe);
            else
                throw new ArgumentNullException(nameof(Recipe));

        }
    }


}
