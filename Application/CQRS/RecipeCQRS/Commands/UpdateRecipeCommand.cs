using Application.Dtos;
using Application.IRepositories;
using MediatR;
using Presentation.ViewModel.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.RecipeCQRS.Commands
{
    public sealed  record class UpdateRecipeCommand(UpdateRecipeViewModel Model) : IRequest<RecipeDto>;
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRecipeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RecipeDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.Model.Id);
            if (recipe == null)
            {
                throw new Exception("Recipe not found");
            }
            recipe.Name = request.Model.Name;
            recipe.Description = request.Model.Description;
            recipe.Price = request.Model.Price;
            await _unitOfWork.Repository<Domain.Models.Recipe>().UpdateInclude(recipe);
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Price = recipe.Price,
                ImageUrl = recipe.ImageUrl,
                Discount = recipe.Discount,
                Category = recipe.Category.Name
            };
        }
    }

}
