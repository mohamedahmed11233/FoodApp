using Application.CQRS.Recipe.Events;
using Application.CQRS.Recipe.Queries;
using Application.Dtos;
using Application.Dtos.Recipe;
using Application.IRepositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Commands
{
    public sealed record UpdateRecipeCommand(UpdateRecipeDto Model) : IRequest<UpdateRecipeDto>;
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, UpdateRecipeDto>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(IMediator mediator , IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._mediator = mediator;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<UpdateRecipeDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {

            var recipe =await _mediator.Send(new GetRecipeByIdQuery(request.Model.Id));
            await _mediator.Publish(new UpdateRecipeEvent(request.Model));
            var MappedRecipe = _mapper.Map<RecipeDto, Domain.Models.Recipe>(recipe);
            var UpdatedRecipe = _unitOfWork.Repository<Domain.Models.Recipe>().UpdateInclude(MappedRecipe , nameof(MappedRecipe.Price), nameof(MappedRecipe.Description) );
            var updated = _mapper.Map<Domain.Models.Recipe, UpdateRecipeDto>(MappedRecipe);
            return updated;

        }
    }


}
