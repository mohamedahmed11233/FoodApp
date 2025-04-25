using Application.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Category.Commands
{
    public sealed record DeleteCategoryByIdCommand(int Id) : IRequest<bool>;
    public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, bool>
    {
        private readonly    IUnitOfWork _unitOfWork;
        public DeleteCategoryByIdCommandHandler(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repository<Domain.Models.Category>().GetByIdAsync(request.Id);
            if (category is null)
            {
                return false;
            }
            await  _unitOfWork.Repository<Domain.Models.Category>().DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
