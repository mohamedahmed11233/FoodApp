using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Dtos.Recipe;
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
            #region 1st Attempt
            //    CreateMap<Recipe, RecipeDto>()
            //        .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PictureUrlResolver>())
            //        .ForMember(opt => opt.Category, opt => opt.MapFrom(S => S.Category.Name));
            //    CreateMap<Category, CategoryDto>().ReverseMap();
            //    CreateMap<AddRecipeViewModel, RecipeDto>();
            //    CreateMap<RecipeViewModel, AddRecipeDto>().ReverseMap();
            //    CreateMap<AddRecipeDto, Recipe>().ForPath(R=>R.Category.Name , options=>options.MapFrom(Dest=>Dest.Category.ToString()));
            //    CreateMap<Recipe, AddRecipeDto>();
            //    CreateMap<Recipe, RecipeViewModel>();
            //    CreateMap<AddRecipeDto, RecipeViewModel>().ReverseMap();
            //    CreateMap<UpdateRecipeViewModel, RecipeDto>();
            //    CreateMap<RecipeDto, RecipeViewModel>().ReverseMap();
            //    CreateMap<Category, CategoryDto>().ReverseMap();
            //    CreateMap<ResponseViewModel<RecipeDto>, Recipe>().ReverseMap(); 
            #endregion
            CreateMap<AddRecipeViewModel, AddRecipeDto>().ReverseMap();
            CreateMap<Recipe, AddRecipeDto>().ReverseMap();
            CreateMap<Recipe, RecipeDto>().ReverseMap();
            CreateMap<RecipeDto, RecipeViewModel>();
            CreateMap<Recipe, UpdateRecipeDto>();
            CreateMap<UpdateRecipeViewModel, UpdateRecipeDto>();
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
