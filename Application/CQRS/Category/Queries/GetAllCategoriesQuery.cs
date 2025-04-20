using Application.Dtos.Category;
using Application.IRepositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Category.Queries
{
    public sealed record GetAllCategoriesQuery() : IRequest<IList<CategoryDto>>;
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IList<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IList<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.Repository<Domain.Models.Category>().Get(null! ,x=>x.Recipe);

            var mappedCategories = _mapper.Map<IList<CategoryDto>>(categories);
            return mappedCategories;
        }

        
    }
}
