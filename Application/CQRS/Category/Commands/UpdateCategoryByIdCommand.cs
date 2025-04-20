using Application.Dtos.Category;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Category.Commands
{
    public sealed record  UpdateCategoryByIdCommand(UpdateCategoryDto Model) :IRequest<UpdateCategoryDto>;
    public class UpdateCategoryByIdCommandHandler : IRequestHandler<UpdateCategoryByIdCommand, UpdateCategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryByIdCommandHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<UpdateCategoryDto> Handle(UpdateCategoryByIdCommand request, CancellationToken cancellationToken)
        {
            
            var category = await _unitOfWork.Repository<Domain.Models.Category>().GetByIdAsync(request.Model.Id);
            if (category is null)
            {
                return null!;
            }
            var UpdatedCategory = _mapper.Map(request.Model, category);
            await _unitOfWork.Repository<Domain.Models.Category>().UpdateInclude(category);
            await _unitOfWork.SaveChangesAsync();
            var mappedCategory = _mapper.Map<UpdateCategoryDto>(UpdatedCategory);
            return mappedCategory;
        }
    }


}
