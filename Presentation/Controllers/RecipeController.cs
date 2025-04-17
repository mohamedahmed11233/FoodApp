using Application.CQRS.Recipe.Queries;
using Application.CQRS.RecipeCQRS.Commands;
using Application.CQRS.RecipeCQRS.Queries;
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

        public RecipeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<ResponseViewModel<RecipeViewModel>> GetRecipeById(int id)
        {
            var recipe = await _mediator.Send(new GetRecipeByIdQuery(id));

            var mappedRecipe = _mapper.Map<RecipeViewModel>(recipe);
            return new ResponseViewModel<RecipeViewModel>
            (
                success: true,
                message: null,
                data: mappedRecipe
            );
        }

        [HttpGet("GetAllRecipes")]
        public async Task<ResponseViewModel<List<RecipeViewModel>>> GetAllRecipes(string Name)
        {
            var recipes = await _mediator.Send(new GetAllRecipiesQuery(Name));
            var mappedRecipes = _mapper.Map<List<RecipeViewModel>>(recipes);
            return new ResponseViewModel<List<RecipeViewModel>>
            (
                success: true,
                message: null,
                data: mappedRecipes
            );
        }

        [HttpGet("GetRecipeByCategoryId")]
        //[HasFeature(FeatureEnum.UpdateRecipe)]
        public async Task<ResponseViewModel<RecipeViewModel>> GetRecipeByCategoryId(int CategoryId)
        {
            var recipe = await _mediator.Send(new GetRecipeByCategoryIdQuery(CategoryId));
            var mappedRecipe = _mapper.Map<RecipeViewModel>(recipe);
            return new ResponseViewModel<RecipeViewModel>
            (
                success: true,
                message: null,
                data: mappedRecipe
            );
        }
        [HttpPost("AddRecipe")]
        public async Task<ResponseViewModel<RecipeViewModel>> AddRecipe(RecipeViewModel recipeViewModel)
        {
            var recipe = _mapper.Map<AddRecipeDto>(recipeViewModel);
            var createdRecipe = await _mediator.Send(new AddRecipeCommand(recipe));
            var mappedCreatedRecipe = _mapper.Map<RecipeViewModel>(createdRecipe);
            return new ResponseViewModel<RecipeViewModel>
            (
                success: true,
                message: null,
                data: mappedCreatedRecipe
            );
        }
        [HttpPut("UpdateRecipe")]
        public async Task<ResponseViewModel<RecipeViewModel>> UpdateRecipe( RecipeViewModel recipeViewModel)
        {
            var recipe = _mapper.Map<UpdateRecipeDto>(recipeViewModel);
            var updatedRecipe = await _mediator.Send(new UpdateRecipeCommand(recipe));
            var mappedUpdatedRecipe = _mapper.Map<RecipeViewModel>(updatedRecipe);
            return new ResponseViewModel<RecipeViewModel>
            (
                success: true,
                message: null,
                data: mappedUpdatedRecipe
            );
        }
        [HttpDelete("DeleteRecipe")]
        public async Task<ResponseViewModel<bool>> DeleteRecipe(int id)
        {
            var deletedRecipe = await _mediator.Send(new DeleteRecipeCommand(id));
            return new ResponseViewModel<bool>
            (
                success: true,
                message: null,
                data: deletedRecipe
            );
        }
    }
}
