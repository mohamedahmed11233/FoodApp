using Application.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRecipeService
    {     
        public Task<RecipeDto> AddRecipe(RecipeDto recipe);
        public Task<RecipeDto> UpdateRecipe(RecipeDto recipe);

        public Task<bool> DeleteRecipe(string Name);
        public Task<RecipeDto> GetRecipeByName(string Name);
        public Task<List<RecipeDto>> GetAllRecipes();
        public Task<List<RecipeDto>> GetRecipesByCategory(Category category);

    }
}
