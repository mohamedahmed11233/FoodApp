using Application.CQRS.Favorite.Commands.AddFavorite;
using Application.CQRS.Favorite.Commands.RemoveFavorite;
using Application.CQRS.Favorite.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel.Favorite;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoriteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<ResponseViewModel<string>> AddFavorite([FromBody] AddFavoriteCommand command)
        {
            var result = await _mediator.Send(command);

            return ResponseViewModel<string>.SuccessResult(result, "Favorite added successfully.");
        }

        [HttpPost("remove")]
        public async Task<ResponseViewModel<string>> RemoveFavorite([FromBody] RemoveFavoriteCommand command)
        {
            var result = await _mediator.Send(command);

            return ResponseViewModel<string>.SuccessResult(result, "Favorite removed successfully.");
        }

        [HttpGet("get-by-user/{userId}")]
        public async Task<ResponseViewModel<List<FavoriteViewModel>>> GetFavoritesByUserId(int userId)
        {
            var result = await _mediator.Send(new GetFavoritesByUserIdQuery(userId));

            // Map FavoriteDto to FavoriteViewModel
            var mappedResult = result.Select(dto => new FavoriteViewModel
            {
                Id = dto.Id,
                UserId = dto.UserId,
                RecipeId = dto.RecipeId
            }).ToList();

            return ResponseViewModel<List<FavoriteViewModel>>.SuccessResult(mappedResult, "Favorites retrieved successfully.");
        }
      
        

    }

}
