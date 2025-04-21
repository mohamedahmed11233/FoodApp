using Application.CQRS.Favorite.Commands.AddFavorite;
using Application.CQRS.Favorite.Commands.RemoveFavorite;
using Application.CQRS.Favorite.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavorites(Guid userId)
        {
            var result = await _mediator.Send(new GetFavoritesByUserIdQuery(userId));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }

}
