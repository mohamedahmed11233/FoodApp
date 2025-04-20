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
    public sealed record AddCategoryCommand(AddCategoryDto Model):IRequest<AddCategoryDto>;
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, AddCategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<AddCategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Models.Category>(request.Model);

          await  _unitOfWork.Repository<Domain.Models.Category>().AddAsync(category);
          await  _unitOfWork.SaveChangesAsync();
            var mappedCategory = _mapper.Map<AddCategoryDto>(category);
            return mappedCategory;

        }
    }

}
