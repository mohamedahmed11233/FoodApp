using Application.Dtos;
using Application.Dtos.Recipe;
using Application.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.RecipeCQRS.Queries
{
    public sealed record GetRecipeByCategoryIdQuery(int CategoryId) :IRequest<RecipeDto>;
  public class GetRecipeByCategoryIdQueryHandler: IRequestHandler<GetRecipeByCategoryIdQuery, RecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRecipeByCategoryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RecipeDto> Handle(GetRecipeByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var recipe = await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.CategoryId);
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
