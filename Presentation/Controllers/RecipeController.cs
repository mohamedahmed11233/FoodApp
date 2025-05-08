using Application.CQRS.Recipe.Commands;
using Application.CQRS.Recipe.Queries;
using Application.Dtos.Recipe;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly ILogger<RecipeController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedCache;

        public RecipeController(IMediator mediator , IMapper mapper
            ,ILogger<RecipeController> logger , IMemoryCache cache , IDistributedCache distributedCache)
        {
            this._mediator = mediator;
            this._mapper = mapper;
            this._logger = logger;
            this._cache = cache;
            this._distributedCache = distributedCache;
        }
        [HttpPost("AddRecipe")]
        public async Task<ResponseViewModel<AddRecipeViewModel>> AddRecipe(AddRecipeViewModel model)
        {
            var recipeDto = _mapper.Map<AddRecipeDto>(model);

            var result = await _mediator.Send(new AddRecipeCommand(recipeDto));
            if (result is null)
            {
                _logger.LogInformation("Failed to add recipe: {Model}", model);          // Log the error

                return new ResponseViewModel<AddRecipeViewModel>(
                    success: false,
                    message: "Failed to add recipe",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidRecipeData  // Adjust this if a specific error code is required
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
        [HttpPost]
        public ActionResult<string> TestCache()
        {
            
            var recipe = new Recipe { Description = "value" , Id = 1 };
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(recipe);
            _distributedCache.SetString("noha" , data , new DistributedCacheEntryOptions 
            {
                AbsoluteExpirationRelativeToNow =TimeSpan.FromMinutes(3) 
            });
            return "Ok";
        }
        [HttpGet]
        public ActionResult<string> GetCache()
        {
            if (_cache.TryGetValue("Noha From Caching",out string? value))
            
                return Ok(value);    

            else
            {
                return NotFound("Cache not found");
            }            
        }


    }
}