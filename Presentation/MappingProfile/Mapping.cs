using Application.Dtos;
using AutoMapper;
using Domain.Models;
using Presentation.Helpers;

namespace Presentation.MappingProfile
{
    public class Mapping :Profile
    {
        public Mapping()
        {
            CreateMap<Recipe , RecipeDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PictureUrlResolver>())
                .ForMember(opt=>opt.Category , opt => opt.MapFrom(S=>S.Category.Name));


        }
    }
}
