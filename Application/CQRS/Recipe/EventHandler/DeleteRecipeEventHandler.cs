using Application.CQRS.Recipe.Commands;
using Application.CQRS.Recipe.Events;
using Application.CQRS.Recipe.Queries;
using Application.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.EventHandler
{
    public class DeleteRecipeEventHandler : INotificationHandler<DeleteRecipeEvent>
    {
        private readonly IMediator _mediator;

        public DeleteRecipeEventHandler( IMediator mediator)
        {
           _mediator = mediator;
        }
        public async Task Handle(DeleteRecipeEvent notification, CancellationToken cancellationToken)
        {
            var recipe = await _mediator.Send(new DeleteRecipeByIdCommand(notification.Id));

        }
    }
}
