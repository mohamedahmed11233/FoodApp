using Application.CQRS.Recipe.Commands;
using Application.CQRS.Recipe.Queries;
using Application.Dtos;
using Application.Dtos.Recipe;
using AutoMapper;
using Domain.Enum.SharedEnums;
using Hotel_Reservation_System.Error;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel.Recipes;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RecipeController(IMediator mediator , IMapper mapper )
        {
            this._mediator = mediator;
            this._mapper = mapper;
        }
        [HttpPost("AddRecipe")]
        public async Task<ResponseViewModel<AddRecipeViewModel>> AddRecipe(AddRecipeViewModel model)
        {
            
            var recipeDto = _mapper.Map<AddRecipeDto>(model);

            var result = await _mediator.Send(new AddRecipeCommand(recipeDto));
            if (result is null)
            {
                return new ResponseViewModel<AddRecipeViewModel>(
                    success: false,
                    message: "Failed to add recipe",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidRecipeData // Adjust this if a specific error code is required
                );
            }

            var recipeViewModel = _mapper.Map<AddRecipeViewModel>(result);
            return new ResponseViewModel<AddRecipeViewModel>(
                success: true,
                message: "Recipe added successfully",
                data: recipeViewModel
            );

        }

        [HttpGet("GetAllRecipes")]
        public async Task<ResponseViewModel<IEnumerable<RecipeViewModel>>> GetAllRecipes()
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
            if (recipes is null)
            {
                return new ResponseViewModel<IEnumerable<RecipeViewModel>>
                (
                    success: false,
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.RecipeNotFound
                );
            }

            var recipeViewModels = _mapper.Map<IEnumerable<RecipeViewModel>>(recipes);
            return new ResponseViewModel<IEnumerable<RecipeViewModel>>(
                success: true,
                data: recipeViewModels
            );
        }

        [HttpGet("GetRecipeById")]
        public async Task<ResponseViewModel<RecipeViewModel>> GetRecipeById(int Id)
        {
            var recipe =await _mediator.Send(new GetRecipeByIdQuery(Id));
            if (recipe is null)
            {
                return new ResponseViewModel<RecipeViewModel>
                (
                    success: false,
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.RecipeNotFound
                );
            }
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);
            return new ResponseViewModel<RecipeViewModel>
            (
                success: true,
                data: recipeViewModel
            );
        }

        [HttpDelete("DeleteRecipe")]
        public async Task<ResponseViewModel<bool>> DeleteRecipeById(int Id)
        {
            var recipe = await _mediator.Send(new DeleteRecipeByIdCommand(Id));
            if (!recipe)
            {
                return new ResponseViewModel<bool>
                (
                    success: false,
                    data: recipe,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.FailedDeleteRecipe
                );
            }
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);
            return new ResponseViewModel<bool>
            (
                success: true,
                data: recipe
            );
        }
        [HttpPut("UpdateRecipeById/{model.Id}")]
        public async Task<ResponseViewModel<UpdateRecipeViewModel>> UpdateRecipeById(UpdateRecipeViewModel model)
        {
            var recipeDto = _mapper.Map<UpdateRecipeViewModel, UpdateRecipeDto>(model);
            var recipe = await _mediator.Send(new UpdateRecipeCommand(recipeDto));
            if (recipe is null)
            {
                return new ResponseViewModel<UpdateRecipeViewModel>
                (
                    success: false,
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.FailedUpdateRecipe
                );
            }
            var recipeViewModel = _mapper.Map<UpdateRecipeViewModel>(recipe);
            return new ResponseViewModel<UpdateRecipeViewModel>
            (
                success: true,
                data: recipeViewModel
            );
        }
    }
}
