using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using Domain.IRepositories;
using Domain.Models;

namespace Application.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecipeService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RecipeDto> AddRecipe(RecipeDto recipe)
        {
            if (recipe is null) return null!;
            var mappedRecipe = _mapper.Map<Recipe>(recipe);
            var Addrecipe = _unitOfWork.Repository<Recipe>().AddAsync(mappedRecipe);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                ex.ToString();
            }
            var recipeDto = _mapper.Map<RecipeDto>(Addrecipe);
            return recipeDto;

        }

        public async Task<bool> DeleteRecipe(string Name)
        {
            if (Name is null) return false;

            var recipe = await _unitOfWork.Repository<Recipe>().GetBySpecAsync(X => X.Name == Name);

            if (recipe is null) return false;
            await _unitOfWork.Repository<Recipe>().DeleteAsync(recipe);

            await _unitOfWork.SaveChangesAsync();

           return true;
        }

        public async Task<List<RecipeDto>> GetAllRecipes()
        {
            var recipes = await _unitOfWork.Repository<Recipe>().GetAllWithSpecAsync(X => X.Category != null && !string.IsNullOrEmpty(X.Category.Name));
            if (recipes is null) return null!;
            var recipeDto = _mapper.Map<List<RecipeDto>>(recipes);
            return recipeDto;
        }

        public async Task<RecipeDto> GetRecipeByName(string Name)
        {
            if (Name is null) return null!;
            var recipe =await _unitOfWork.Repository<Recipe>().GetBySpecAsync(X=>X.Name ==Name);
            if (recipe is null) return null!;
            var recipeDto = _mapper.Map<RecipeDto>(recipe);
            return recipeDto;
        }

        public async Task<List<RecipeDto>> GetRecipesByCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Name)) return null!;
            var recipes = await _unitOfWork.Repository<Recipe>().GetBySpecAsync(x => x.Category.Name == category.Name);
            if (recipes is null) return null!;
            var recipeDto = _mapper.Map<List<RecipeDto>>(recipes);
            return recipeDto;
        }

        public async Task<RecipeDto> UpdateRecipe(RecipeDto recipe)
        {

            if (recipe is null) return null!;
            var mappedRecipe = _mapper.Map<Recipe>(recipe);
            var updatedRecipe = _unitOfWork.Repository<Recipe>().UpdateInclude(mappedRecipe, nameof(mappedRecipe.Name), nameof(mappedRecipe.Description), nameof(mappedRecipe.Price), nameof(mappedRecipe.Quantity), nameof(mappedRecipe.ImageUrl), nameof(mappedRecipe.Discount), nameof(mappedRecipe.Category));
            await _unitOfWork.SaveChangesAsync();
            var recipeDto = _mapper.Map<RecipeDto>(updatedRecipe);
            return recipeDto;

        }
    }
}
