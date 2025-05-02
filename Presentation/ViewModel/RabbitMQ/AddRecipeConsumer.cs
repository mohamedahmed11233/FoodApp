
using MediatR;

namespace Presentation.ViewModel.RabbitMQ
{
    public class AddRecipeConsumer : IBaseMessage<AddRecipeMessage>
    {
        private readonly IMediator _mediator;

        public AddRecipeConsumer(IMediator mediator)
        {
            this._mediator = mediator;
        }
        public Task ConsumeAsync(AddRecipeMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
