using Application.Dtos;
using AutoMapper;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel.Recipes;
namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeService recipeService , IMapper mapper)
        {
            _recipeService = recipeService;
            _mapper = mapper;
        }

        [HttpGet("GetAllRecipes")]
        public async Task<ResponseViewModel<List<RecipeViewModel>>> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipes();
            if (recipes is null)
                return ResponseViewModel<List<RecipeViewModel>>.ErrorResult("No recipes found", null!);
            var mappedRecipies = _mapper.Map<List<RecipeViewModel>>(recipes);
            return ResponseViewModel<List<RecipeViewModel>>.SuccessResult(mappedRecipies);
        }
        [HttpGet("GetRecipeByName/{Name}")]
        public async Task<ActionResult<ResponseViewModel<RecipeViewModel>>> GetRecipeByName(string Name)
        {
            var recipe = await _recipeService.GetRecipeByName(Name);
            if (recipe is null)
                return BadRequest(400);
            var mappedRecipe = _mapper.Map<RecipeViewModel>(recipe);
            return ResponseViewModel<RecipeViewModel>.SuccessResult(mappedRecipe);
        }

        [HttpGet("GetRecipeById/{RecipeId}")]
        public async Task<ResponseViewModel<RecipeViewModel>> GetRecipeByName(int RecipeId)
        {
            var recipe = await _recipeService.GetRecipeById(RecipeId);
            if (recipe is null)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("recipe is not found." , null);
            var mappedRecipe = _mapper.Map<RecipeViewModel>(recipe);
            return ResponseViewModel<RecipeViewModel>.SuccessResult(mappedRecipe);
        }

        [HttpPost("AddRecipe")]
        public async Task<ResponseViewModel<RecipeViewModel>> AddRecipe( AddRecipeViewModel recipe)
        {
            if (recipe is null)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("Invalid recipe data", null);
            var maapedRecipe = _mapper.Map<RecipeDto>(recipe);
            var addedRecipe = await _recipeService.AddRecipe(maapedRecipe);
            var RecipeData = _mapper.Map<RecipeViewModel>(addedRecipe);
            if (addedRecipe is null)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("Failed to add recipe", null);
            return ResponseViewModel<RecipeViewModel>.SuccessResult(RecipeData, "Recipe added successfully");
        }
        [HttpPut("UpdateRecipe")]
        public async Task<ResponseViewModel<RecipeViewModel>> UpdateRecipe(UpdateRecipeViewModel recipe)
        {
            if (recipe is null)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("Invalid recipe data", null);
            var maapedRecipe = _mapper.Map<RecipeDto>(recipe);
            var updatedRecipe = await _recipeService.UpdateRecipe(maapedRecipe);
            var RecipeData = _mapper.Map<RecipeViewModel>(updatedRecipe);
            if (updatedRecipe is null)

                return ResponseViewModel<RecipeViewModel>.ErrorResult("Failed to update recipe", null);
            return ResponseViewModel<RecipeViewModel>.SuccessResult(RecipeData);
        }
        [HttpDelete("DeleteRecipe/{Name}")]
        public async Task<ResponseViewModel<RecipeViewModel>> DeleteRecipe(string RecipeName)
        {
            if (RecipeName is null)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("Invalid recipe name" , null!);
            var deleted = await _recipeService.DeleteRecipe(RecipeName);

            if (!deleted)
                return ResponseViewModel<RecipeViewModel>.ErrorResult(null , null );
            return ResponseViewModel<RecipeViewModel>.SuccessResult(null, "Recipe deleted successfully");
        }
        [HttpDelete("DeleteRecipeById/{RecipeId}")]
        public async Task<ResponseViewModel<RecipeViewModel>> DeleteRecipe(int RecipeId)
        {
            if (RecipeId ==0)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("Invalid recipe name" , null!);
            var deleted = await _recipeService.DeleteRecipeById(RecipeId);
            if (!deleted)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("Failed to delete recipe", null!);
            return ResponseViewModel<RecipeViewModel>.SuccessResult( null! , "Recipe deleted successfully" );
        }
        [HttpGet("GetRecipeByCategory/{CategoryId}")]
        public async Task<ResponseViewModel<RecipeViewModel>> GetRecipeByCategoryId(int CategoryId)
        {
            var Recipe = await _recipeService.GetRecipesByCategory(CategoryId);
            var RecipeData = _mapper.Map<RecipeViewModel>(Recipe);
            if (Recipe is null)
                return ResponseViewModel<RecipeViewModel>.ErrorResult("No recipes found in this category", RecipeData);
            return ResponseViewModel<RecipeViewModel>.SuccessResult(RecipeData);
        } 
    }
}
