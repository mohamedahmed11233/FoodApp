using Application.CQRS.Auth.Commands.OTP;
using Application.CQRS.Auth.Commands.ResetPassword;
using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Dtos.Category;
using Application.Dtos.Recipe;
using Application.Dtos.User;
using AutoMapper;

using Domain.Models;
using Presentation.Helpers;
using Presentation.ViewModel;
using Presentation.ViewModel.Auth;
using Presentation.ViewModel.Category;
using Presentation.ViewModel.Recipes;

namespace Presentation.MappingProfile
{
    public class Mapping : Profile
    {
        public Mapping()
        {

            CreateMap<AddRecipeViewModel, AddRecipeDto>().ReverseMap();
            CreateMap<Recipe, AddRecipeDto>().ReverseMap();
            CreateMap<Recipe, RecipeDto>().ReverseMap();
            CreateMap<RecipeDto, RecipeViewModel>();
            CreateMap<Recipe, UpdateRecipeDto>();
            CreateMap<UpdateRecipeViewModel, UpdateRecipeDto>();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
            CreateMap<AddCategoryDto, Category>().ReverseMap();
            CreateMap<AddCategoryViewModel, AddCategoryDto>().ReverseMap();
            CreateMap<AddCategoryViewModel, AddCategoryDto>().ReverseMap();
            CreateMap<AddCategoryDto, CategoryViewModel>().ReverseMap();
            CreateMap<UpdateCategoryViewModel, UpdateCategoryDto>();
            CreateMap<UpdateCategoryDto, CategoryViewModel>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryDto, CategoryViewModel>().ReverseMap();
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

            CreateMap<VerifyOtpRequestViewModel, VerifyOtpRequestDto>();
            CreateMap<VerifyOtpRequestViewModel, VerifyOtpCodeCommand>()
                .ConstructUsing(src => new VerifyOtpCodeCommand(new VerifyOtpRequestDto
                {
                    UserId = src.UserId,
                    Code = src.Code
                }));

            CreateMap<ResetPasswordRequestVM, ResetPasswordCommand>()
              .ConstructUsing(src => new ResetPasswordCommand(new ResetPasswordRequestDto
              {
                  UserId = src.UserId,
                  OtpCode = src.OtpCode,
                  NewPassword = src.NewPassword
              }));
            #endregion

            #region User

            CreateMap<UserDto, UserViewModel>().ReverseMap();
            #endregion
        }
    }
}
