using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Presentation.ViewModel.Auth;
using AutoMapper;
using Application.Dtos.Auth;
using Domain.Enum.SharedEnums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Application.CQRS.Auth.Commend.AssignRoleToUser;
using Application.CQRS.Auth.Commend.AssignFeatures;
using Presentation.ViewModel;
using Application.CQRS.Auth.Queries.UserDataOperation;
using Application.Dtos.User;

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
                return ResponseViewModel<RegisterResponseDto>.SuccessResult(result, "User registered successfully");
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message, null, ErrorCode.FailedRegister);
        }

        [HttpPost("login")]
        public async Task<ResponseViewModel<RegisterResponseDto>> Login(LoginViewModel model)
        {
            var command = _mapper.Map<LoginUserCommand>(model);

            var result = await _mediator.Send(command);

            if (result.IsSucess)
                return ResponseViewModel<RegisterResponseDto>.SuccessResult(result, "Login successful");
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message, null, ErrorCode.FailedLogin);
        }
        [HttpPost("AssignRole")]
        public async Task<ResponseViewModel<bool>> AssignRole([FromBody] AssignRoleToUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
                return ResponseViewModel<bool>.SuccessResult(true, "Role assigned successfully");

            return ResponseViewModel<bool>.ErrorResult("User not found", false, ErrorCode.UserNotFound);
        }


        [HttpPost("AssignRoleFeatures")]
        public async Task<ResponseViewModel<bool>> AssignRoleFeatures(AssignRoleFeaturesCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
                return ResponseViewModel<bool>.SuccessResult(true, "Features assigned successfully");

            return ResponseViewModel<bool>.ErrorResult("Failed to assign features", false, ErrorCode.internalServer);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ResponseViewModel<IEnumerable<UserViewModel>>> GetAllUser()
        {
            var user = await _mediator.Send( new GetAllUserQuery());
            var result = _mapper.Map<IEnumerable<UserViewModel>>(user);
            return ResponseViewModel<IEnumerable<UserViewModel>>.SuccessResult(result, "Users retrieved successfully");
        }

        [HttpGet("GetUser")]
        public async Task<ResponseViewModel<UserViewModel>> GetUserByID(int Id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(Id));
            var result = _mapper.Map<UserViewModel>(user);
            return ResponseViewModel<UserViewModel>.SuccessResult(result, "Users retrieved successfully");
        }

    }
}