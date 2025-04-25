using Application.CQRS.Favorite.Commands;
using Application.CQRS.Favorite.Queries;
using Application.CQRS.Recipe.Queries;
using Application.Dtos.Recipe;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel.Recipes;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FavoriteController(IMediator mediator , IMapper mapper)
        {
            this._mediator = mediator;
            this._mapper = mapper;
        }
        [HttpGet("GetAllFavoriteItems")]
        public async Task<ResponseViewModel<IEnumerable<RecipeViewModel>>> GetAllRecipes()
        {
            var recipes = await _mediator.Send(new GetAllFavoriteItemsQuery());
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
        [HttpGet("GetFavoriteItem/{Id}")]
        public async Task<ResponseViewModel<RecipeViewModel>> GetFavoriteItemById(int Id)
        {
            var recipe = await _mediator.Send(new GetFavoriteItemByIdQuery(Id));
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
        [HttpPost("AddToFavorite/{Id}")]
        public async Task<ResponseViewModel<bool>> AddToFavorite(int Id)
        {
            var result = await _mediator.Send(new AddFavoriteItemCommand(Id));
            return new ResponseViewModel<bool>
            (
                success: true,
                message: "Recipe added to favorites successfully"
            );
        }
        [HttpDelete("DeleteFavorite/{Id}")]
        public async Task<ResponseViewModel<bool>> DeleteFromFavorite(int Id)
        {
            var result =await _mediator.Send(new DeleteFavoriteItemCommand(Id));
            return new ResponseViewModel<bool>
          (
              success: true,
              message: "Recipe deleted from favorites successfully"
          );
        }

    }
}
