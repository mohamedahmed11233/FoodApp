using Application.Dtos.Recipe;
using Application.IRepositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Commands
{
    public sealed record AddFavoriteItemCommand(int RecipeId):IRequest<AddRecipeDto>;
    public class AddFavoriteItemCommandHandler : IRequestHandler<AddFavoriteItemCommand , AddRecipeDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddFavoriteItemCommandHandler(IMapper mapper , IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<AddRecipeDto> Handle(AddFavoriteItemCommand request, CancellationToken cancellationToken)
        {
            // Fetch the recipe from the repository
            var addedRecipeTask = _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.RecipeId);
            var addedRecipe = await addedRecipeTask; 

            if (addedRecipe is null) return null!; // Handle the case where the recipe is not found

            // Update the IsFavorite property
            addedRecipe.IsFavorite = true;

            // Save changes to the database
            await _unitOfWork.SaveChangesAsync();

            // Map the updated Recipe back to the DTO
            var mappedRecipe = _mapper.Map<Domain.Models.Recipe, AddRecipeDto>(addedRecipe);
            return mappedRecipe;
        }
    }


}
