using Application.CQRS.Recipe.Commands;
using Application.CQRS.Recipe.Events;
using Application.Dtos.Recipe;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.EventHandler
{
    public class UpdateRecipeEventHandler : INotificationHandler<UpdateRecipeEvent>
    {
        private readonly IMediator _mediator;

        public UpdateRecipeEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UpdateRecipeEvent notification, CancellationToken cancellationToken)
        {
            var updateRecipeDto = new UpdateRecipeDto
            {
                Id = notification.model.Id, 
                Name = notification.model.Name,   
                Description = notification.model.Description, 
                Price = notification.model.Price  
            };

            var recipe = await _mediator.Send(new UpdateRecipeCommand(updateRecipeDto));
        }
    }
}
