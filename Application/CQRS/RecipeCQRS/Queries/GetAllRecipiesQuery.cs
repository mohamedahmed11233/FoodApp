using Application.Dtos;
using Application.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.RecipeCQRS.Queries
{
    public sealed record GetAllRecipiesQuery(string Name) : IRequest<List<RecipeDto>>;

    public class GetAllRecipiesQueryHandler : IRequestHandler<GetAllRecipiesQuery, List<RecipeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllRecipiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RecipeDto>> Handle(GetAllRecipiesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _unitOfWork.Repository<Domain.Models.Recipe>().GetAllAsync();
            var recipeDtos = recipes.Select(recipe => new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Price = recipe.Price,
                ImageUrl = recipe.ImageUrl,
                Discount = recipe.Discount,
                Category = recipe.Category.Name
            }).ToList();
            return recipeDtos;
        }
    }
}
