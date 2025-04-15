using Application.Dtos;
using Application.Dtos.Recipe;
using Application.IRepositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Queries
{
    public sealed record GetRecipeByIdQuery(int RecipeId) : IRequest<RecipeDto>;

    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRecipeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        async Task<RecipeDto> IRequestHandler<GetRecipeByIdQuery, RecipeDto>.Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            var recipe = await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.RecipeId);
            if (recipe == null)
            {
                throw new Exception("Recipe not found");
            }
            var recipeDto = new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Price = recipe.Price,
                ImageUrl = recipe.ImageUrl,
                Discount = recipe.Discount,
                Category = recipe.Category.Name
            };
            return recipeDto;
        }
    }
}
