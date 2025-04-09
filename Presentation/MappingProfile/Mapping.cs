using Application.Dtos;
using AutoMapper;
using Domain.Dtos;
using Domain.Models;
using Presentation.Helpers;
using Presentation.ViewModel.Recipes;

namespace Presentation.MappingProfile
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PictureUrlResolver>())
                .ForMember(opt => opt.Category, opt => opt.MapFrom(S => S.Category.Name));
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<RecipeDto, Recipe>()
                .AfterMap((src, dest) =>
                {
                    if (dest.Category is null)
                    {
                        dest.Category = new Category();
                    }
                    dest.Category.Name = src.Category;
                });
            CreateMap<AddRecipeViewModel, RecipeDto>();
            CreateMap<UpdateRecipeViewModel, RecipeDto>();
            CreateMap<ResponseViewModel<RecipeDto>, Recipe>().ReverseMap();
        }
    }
}
