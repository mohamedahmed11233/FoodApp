using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Application.ViewModel.Auth;
using AutoMapper;
using Domain.Dtos.Auth;
using Domain.Enum.SharedEnums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("Register")]

        public async Task<ResponseViewModel<RegisterResponseDto>> Register(RegisterViewModel model)
        {
            var command = _mapper.Map<RegisterUserCommand>(model);
            var result = await _mediator.Send(command);

            if (result.IsSucess)
            {
                return ResponseViewModel<RegisterResponseDto>.SuccessResult(result, "User registered successfully");
            }
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message, null, ErrorCode.FailedRegister);
        }


        [HttpPost("login")]
        public async Task<ResponseViewModel<RegisterResponseDto>> Login(LoginViewModel model)
        {
            var command = _mapper.Map<LoginUserCommand>(model);
            
            var result = await _mediator.Send(command);

            if (result.IsSucess)
            {
                return ResponseViewModel<RegisterResponseDto>.SuccessResult(result, "Login successful");
            }
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message, null, ErrorCode.FailedLogin);
        }

    }
}