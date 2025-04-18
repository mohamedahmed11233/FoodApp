using Application.CQRS.Recipe.Events;
using Application.CQRS.Recipe.Queries;
using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Commands
{
    public sealed record DeleteRecipeByIdCommand(int id):IRequest<bool>;
    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeByIdCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteRecipeCommandHandler(IMediator mediator , IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._mediator = mediator;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<bool> Handle(DeleteRecipeByIdCommand request, CancellationToken cancellationToken)
        {
            var RecipeDto = await _mediator.Send(new GetRecipeByIdQuery(request.id));
            if (RecipeDto is null)
            {
                return false; 
            }

            var recipe = _mapper.Map<Domain.Models.Recipe>(RecipeDto);

            await _mediator.Publish(new DeleteRecipeEvent(request.id));
             await _unitOfWork.Repository<Domain.Models.Recipe>().DeleteAsync(recipe);
            await _unitOfWork.SaveChangesAsync();
            return true; 
        }

    }
}
