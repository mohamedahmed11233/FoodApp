using Application.Dtos;
using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using Domain.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace Presentation.Helpers
{
    public class PictureUrlResolver : IValueResolver<Recipe, RecipeDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Recipe source, RecipeDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{_configuration["DefaultImageUrl"]}{source.ImageUrl}";

            }
            return string.Empty;
        }
    }
}
