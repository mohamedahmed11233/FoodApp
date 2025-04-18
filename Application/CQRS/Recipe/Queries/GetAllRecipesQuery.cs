using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Queries
{
    public sealed record GetAllRecipesQuery(string ?Name) :IRequest<IEnumerable<RecipeDto>>;
    public class GetAllRecipesQueryHandler :IRequestHandler<GetAllRecipesQuery , IEnumerable<RecipeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRecipesQueryHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<RecipeDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _unitOfWork.Repository<Domain.Models.Recipe>().GetAllAsync();
            if (request.Name != null)
            {
              await _unitOfWork.Repository<Domain.Models.Recipe>().GetAllWithSpecAsync(x => x.Name == request.Name);
            }
            
            var recipeDtos  = _mapper.Map<IEnumerable<RecipeDto>>(recipes);
            return recipeDtos;
        }
    }

}
