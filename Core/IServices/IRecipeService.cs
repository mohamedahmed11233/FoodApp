using Application.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IRecipeService
    {
        public Task<RecipeDto> AddRecipe(RecipeDto recipe);
        public Task<RecipeDto> UpdateRecipe(RecipeDto recipe);
        public Task<RecipeDto> GetRecipeById(int RecipeId);
        public Task<bool> DeleteRecipe(string Name);
        public  Task<bool> DeleteRecipeById(int Id);
        public Task<RecipeDto> GetRecipeByName(string Name);
        public Task<List<RecipeDto>> GetAllRecipes();
        public Task<List<RecipeDto>> GetRecipesByCategory(int categoryId);

    }
}
