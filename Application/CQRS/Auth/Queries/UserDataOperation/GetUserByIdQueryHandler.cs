using Application.Dtos.User;
using AutoMapper;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Queries.UserDataOperation
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IGenericRepository<User> userRepository ,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserID);
            if (user == null)
                throw new Exception("User Not Found");
            var result= _mapper.Map<UserDto>(user);
            return result;
        }
    }
}
