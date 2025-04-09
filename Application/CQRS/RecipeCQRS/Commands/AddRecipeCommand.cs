using Application.Dtos;
using Application.IRepositories;
using AutoMapper;
using MediatR;
using Presentation.ViewModel.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.RecipeCQRS.Commands
{
    public sealed record AddRecipeCommand(AddRecipeViewModel Model) : IRequest<AddRecipeViewModel>;

    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, AddRecipeViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddRecipeCommandHandler(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public Task<AddRecipeViewModel> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            var Recipe = _mapper.Map<Domain.Models.Recipe>(request.Model);
            var Addedrecipe = _unitOfWork.Repository<Domain.Models.Recipe>().AddAsync(Recipe);
            if(Addedrecipe is null)
            {
                throw new Exception("Recipe not found");
            }
            return Task.FromResult(_mapper.Map<AddRecipeViewModel>(Addedrecipe));
        }
    }


}
