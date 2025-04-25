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
    public sealed record  GetAllFavoriteItemsQuery:IRequest<IList<RecipeDto>>;

    public class GetAllFavoriteItemsQueryHandler : IRequestHandler<GetAllFavoriteItemsQuery, IList<RecipeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllFavoriteItemsQueryHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IList<RecipeDto>> Handle(GetAllFavoriteItemsQuery request, CancellationToken cancellationToken)
        {
            if(request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var Items = await _unitOfWork.Repository<Domain.Models.Recipe>().Get(R => R.IsFavorite == true);
            if (Items is null)
            {
                throw new ArgumentNullException(nameof(Items));
            }
            var recipeDto = _mapper.Map<IList<RecipeDto>>(Items);
            return recipeDto;   

        }
    }

}
