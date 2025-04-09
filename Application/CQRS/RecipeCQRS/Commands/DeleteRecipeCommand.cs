using Application.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.RecipeCQRS.Commands
{
   public sealed record DeleteRecipeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteRecipeCommand(int id)
        {
            Id = id;
        }
    }
    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRecipeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.Id);
            if (recipe == null)
            {
                throw new Exception("Recipe not found");
            }
            await _unitOfWork.Repository<Domain.Models.Recipe>().DeleteAsync(recipe);
            return true;
        }
    }

}
