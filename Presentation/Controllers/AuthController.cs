using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Application.ViewModel.Auth;
using AutoMapper;
using Domain.Dtos.Auth;
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

        [HttpPost("register")]
        public async Task<ResponseViewModel<AuthDto>> Register(RegisterViewModel model)
        {
            // Map view model to command using AutoMapper
            var command = _mapper.Map<RegisterUserCommand>(model);
            var result = await _mediator.Send(command);

            if (result.IsAuthenticated)
            {
                return ResponseViewModel<AuthDto>.SuccessResult(result, result.Message);
            }
            else
            {
                return ResponseViewModel<AuthDto>.ErrorResult(result.Message, result);
            }
        }

        [HttpPost("login")]
        public async Task<ResponseViewModel<AuthDto>> Login(LoginViewModel model)
        {
            var command = _mapper.Map<LoginUserCommand>(model);
            var result = await _mediator.Send(command);

            if (result.IsAuthenticated)
            {
                return ResponseViewModel<AuthDto>.SuccessResult(result, result.Message);
            }
            else
            {
                return ResponseViewModel<AuthDto>.ErrorResult(result.Message, result);
            }
        }
    }
}