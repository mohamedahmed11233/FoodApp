using Application.Dtos.User;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.MappingProfileDto
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
