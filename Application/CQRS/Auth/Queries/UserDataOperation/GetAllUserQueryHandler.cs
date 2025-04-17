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
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserDto>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserQueryHandler(IGenericRepository<User> userRepository ,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var user= await _userRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<UserDto>>(user);
            return result;
        }
    }
}
