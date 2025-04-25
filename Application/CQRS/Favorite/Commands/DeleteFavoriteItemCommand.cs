using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Commands
{
  public sealed record DeleteFavoriteItemCommand(int RecipeId) :IRequest<RecipeDto>;

    public class DeleteFavoriteItemCommandHandler : IRequestHandler<DeleteFavoriteItemCommand, RecipeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteFavoriteItemCommandHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<RecipeDto> Handle(DeleteFavoriteItemCommand request, CancellationToken cancellationToken)
        {
            if (request.RecipeId == 0) return null!;
            var recipe = await _unitOfWork.Repository<Domain.Models.Recipe>().GetByIdAsync(request.RecipeId);
            if (recipe is null) return null!;
            recipe.IsFavorite = false;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RecipeDto>(recipe);
        }
    }
}
