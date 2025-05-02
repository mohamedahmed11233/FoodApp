using Application.Dtos;
using Application.Dtos.Recipe;
using Application.IRepositories;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Commands
{
    public sealed record AddRecipeCommand(AddRecipeDto Model):IRequest<AddRecipeDto>;
    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, AddRecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddRecipeCommandHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<AddRecipeDto> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = new Domain.Models.Recipe 
            {
                Name = request.Model.Name ,
                Description = request.Model.Description,
                Price = request.Model.Price,
                ImageUrl = request.Model.ImageUrl,
                Discount = request.Model.Discount,
                Category = new Domain.Models.Category
                {
                    Name = request.Model.Category,
                    Description = request.Model.Description,
                },
                Quantity = 1,

            };
           await _unitOfWork.Repository<Domain.Models.Recipe>().AddAsync(recipe);
           await _unitOfWork.SaveChangesAsync();
          
            var mappedRecipe = _mapper.Map<Domain.Models.Recipe, AddRecipeDto>(recipe);
            return mappedRecipe;

        }
    }

}
