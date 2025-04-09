using Application.Dtos;
using AutoMapper;
using Domain.IServices;
using Domain.Models;
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
        public async Task<ResponseViewModel<List<RecipeDto>>> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipes();
            if (recipes is null)
                return ResponseViewModel<List<RecipeDto>>.ErrorResult("No recipes found", null);
            return ResponseViewModel<List<RecipeDto>>.SuccessResult(recipes);
        }
        [HttpGet("GetRecipeByName/{Name}")]
        public async Task<ResponseViewModel<RecipeDto>> GetRecipeByName(string Name)
        {
            var recipe = await _recipeService.GetRecipeByName(Name);
            if (recipe is null)
                return ResponseViewModel<RecipeDto>.ErrorResult("No recipe found", null);
            return ResponseViewModel<RecipeDto>.SuccessResult(recipe);
        }

        [HttpPost("AddRecipe")]
        public async Task<ResponseViewModel<RecipeDto>> AddRecipe( AddRecipeViewModel recipe)
        {
            if (recipe is null)
                return ResponseViewModel<RecipeDto>.ErrorResult("Invalid recipe data", null);
            var maapedRecipe = _mapper.Map<RecipeDto>(recipe);
            var addedRecipe = await _recipeService.AddRecipe(maapedRecipe);
            if (addedRecipe is null)
                return ResponseViewModel<RecipeDto>.ErrorResult("Failed to add recipe", null);
            return ResponseViewModel<RecipeDto>.SuccessResult(addedRecipe, "Recipe added successfully");
        }
        [HttpPut("UpdateRecipe")]
        public async Task<ResponseViewModel<RecipeDto>> UpdateRecipe(UpdateRecipeViewModel recipe)
        {
            if (recipe is null)
                return ResponseViewModel<RecipeDto>.ErrorResult("Invalid recipe data", null);
            var maapedRecipe = _mapper.Map<RecipeDto>(recipe);
            var updatedRecipe = await _recipeService.UpdateRecipe(maapedRecipe);
            if (updatedRecipe is null)
                return ResponseViewModel<RecipeDto>.ErrorResult("Failed to update recipe", null);
            return ResponseViewModel<RecipeDto>.SuccessResult(updatedRecipe);
        }
        [HttpDelete("DeleteRecipe/{Name}")]

        public async Task<ResponseViewModel<bool>> DeleteRecipe(string RecipeName)
        {
            if (RecipeName is null)
                return ResponseViewModel<bool>.ErrorResult("Invalid recipe name", false);
            var deleted = await _recipeService.DeleteRecipe(RecipeName);
            if (!deleted)
                return ResponseViewModel<bool>.ErrorResult("Failed to delete recipe", false);
            return ResponseViewModel<bool>.SuccessResult(true, "Recipe deleted successfully");
        }
    }
}
