using Application.Dtos.Category;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Category.Queries
{
    public sealed record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

  

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<CategoryDto> (await _unitOfWork.Repository<Domain.Models.Category>().GetByIdAsync(request.Id));
            if (category is null)
            {
                return null!;
            }
            return category;

        }
    }


}
