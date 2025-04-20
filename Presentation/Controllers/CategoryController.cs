using Application.CQRS.Category.Commands;
using Application.CQRS.Category.Queries;
using Application.Dtos.Category;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel.Category;
using Presentation.ViewModel.Recipes;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryController(IMediator mediator , IMapper mapper)
        {
            this._mediator = mediator;
            this._mapper = mapper;
        }
        [HttpGet("GetAllCategories")]
        public async Task<ResponseViewModel<List<CategoryViewModel>>> GetAllCategories()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            if (result is null)
            {
                return new ResponseViewModel<List<CategoryViewModel>>
                (
                    success: false,
                    message: "Failed to get categories",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            }
            var categoryViewModel = _mapper.Map<IList<CategoryDto>, List<CategoryViewModel>>(result);
            return new ResponseViewModel<List<CategoryViewModel>>
            (
                success: true,
                data: categoryViewModel
            );
        }
        [HttpGet("GetCategoryById/{Id}")]
        public async Task<ResponseViewModel<CategoryViewModel>> GetCategoryById(int Id)
        {
            if (Id == 0)
                return new ResponseViewModel<CategoryViewModel>
                (
                    success: false,
                   message: "category is null.",
                   data: null,
                   errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            var result = await _mediator.Send(new GetCategoryByIdQuery(Id));
            if (result is null)
            {
                return new ResponseViewModel<CategoryViewModel>
                (
                    success: false,
                    message: "Failed to get category",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            }
            var categoryViewModel = _mapper.Map<CategoryDto, CategoryViewModel>(result);
            return new ResponseViewModel<CategoryViewModel>
            (
                success: true,
                message: "Category retrieved successfully",
                data: categoryViewModel
            );
        }   

        [HttpPost("AddCategory")]
        public async Task<ResponseViewModel<CategoryViewModel>> AddCategory(AddCategoryViewModel model)
        {
            if (model is null)
                return new ResponseViewModel<CategoryViewModel>
                (
                    success: false,
                   message: "category is null.",
                   data: null,
                   errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                ); 
            var CategoryDto = _mapper.Map<AddCategoryViewModel, AddCategoryDto>(model);
            var result = await _mediator.Send(new AddCategoryCommand(CategoryDto));
            if(result is null)
            {
                return new ResponseViewModel<CategoryViewModel>
                (
                    success: false,
                    message: "Failed to add category",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            }
            var categoryViewModel = _mapper.Map<AddCategoryDto, CategoryViewModel>(result);
            return new ResponseViewModel<CategoryViewModel>
            (
                success: true,
                message: "Category added successfully",
                data: categoryViewModel
            );

        }

        [HttpPut("UpdateCategory/{Id}")]
        public async Task<ResponseViewModel<CategoryViewModel>> UpdateCategory(UpdateCategoryViewModel model)
        {

            if (model.Id == 0|| model is null )
                return new ResponseViewModel<CategoryViewModel>
                (
                    success: false,
                   message: "category is null.",
                   data: null,
                   errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            var CategoryDto = _mapper.Map<UpdateCategoryViewModel, UpdateCategoryDto>(model);
            var result = await _mediator.Send(new UpdateCategoryByIdCommand(CategoryDto));
            if (result is null)
            {
                return new ResponseViewModel<CategoryViewModel>
                (
                    success: false,
                    message: "Failed to update category",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            }
            var categoryViewModel = _mapper.Map<UpdateCategoryDto, CategoryViewModel>(result);
            return new ResponseViewModel<CategoryViewModel>
            (
                success: true,
                message: "Category updated successfully",
                data: categoryViewModel
            );
        }
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ResponseViewModel<bool>> DeleteCategory(int Id)
        {
            if (Id == 0)
                return new ResponseViewModel<bool>
                (
                    success: false,
                   message: "category is null.",
                   data: false,
                   errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            var result = await _mediator.Send(new DeleteCategoryByIdCommand(Id));
            if (result is false)
            {
                return new ResponseViewModel<bool>
                (
                    success: false,
                    message: "Failed to delete category",
                    data: false,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidCategoryData
                );
            }
            return new ResponseViewModel<bool>
            (
                success: true,
                message: "Category deleted successfully",
                data: true
            );
        }
    }
}
