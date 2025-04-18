using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Queries
{
    public sealed record GetRecipeByIdQuery(int Id) :IRequest<RecipeDto>;

    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery , RecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRecipeByIdQueryHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<RecipeDto> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            var recipe =await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.Id);
            if(recipe is null)
            {
                throw new Exception("Recipe not found");
            }
            var recipeDto = _mapper.Map<RecipeDto>(recipe);
            return (recipeDto);
        }
    }
   
}
