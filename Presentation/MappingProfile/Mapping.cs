using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Dtos.User;
using AutoMapper;

using Domain.Models;
using Presentation.Helpers;
using Presentation.ViewModel;
using Presentation.ViewModel.Auth;
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
            CreateMap<RecipeDto, RecipeViewModel>().ReverseMap();

            CreateMap<ResponseViewModel<RecipeDto>, Recipe>().ReverseMap();


            #region Aurh
            CreateMap<RegisterViewModel, RegisterDto>();

            CreateMap<RegisterViewModel, RegisterUserCommand>()
                .ConstructUsing(src => new RegisterUserCommand(new RegisterDto
                {
                    Username = src.Username,
                    Email = src.Email,
                    Password = src.Password,
                    FirstName = src.FirstName,
                    LastName = src.LastName
                }));
            CreateMap<LoginViewModel, LoginUserCommand>()
            .ConstructUsing(src => new LoginUserCommand(src.Email, src.Password));

            #endregion

            #region User

            CreateMap<UserDto,UserViewModel>() .ReverseMap();
            #endregion
        }
    }
}
