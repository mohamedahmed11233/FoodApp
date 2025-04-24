using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Presentation.ViewModel.Auth;
using AutoMapper;
using Application.Dtos.Auth;
using Domain.Enum.SharedEnums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel;
using Application.CQRS.Auth.Queries.UserDataOperation;
using Application.CQRS.Auth.Queries.OTP;
using Application.CQRS.Auth.Commands.OTP;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                return ResponseViewModel<RegisterResponseDto>.SuccessResult(result);
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message);
        }

        [HttpPost("Login")]
        public async Task<ResponseViewModel<RegisterResponseDto>> Login(LoginViewModel model)
        {
            var command = _mapper.Map<LoginUserCommand>(model);

            var result = await _mediator.Send(command);

            if (result.IsSucess)
                return ResponseViewModel<RegisterResponseDto>.SuccessResult(result);
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message, null, ErrorCode.FailedLogin);
        }

        [HttpGet("generate-otp-secret")]
        public async Task<ResponseViewModel<GenerateSecretKeyDTO>> GenerateOtpSecretAsync(int userId)
        {
            var secretKeyDto = await _mediator.Send(new GenerateSecretKeyQuery(userId));
            if (secretKeyDto is null)
                return ResponseViewModel<GenerateSecretKeyDTO>.ErrorResult("Failed to generate OTP secret key.", null, ErrorCode.NotFound);
            return ResponseViewModel<GenerateSecretKeyDTO>.SuccessResult(secretKeyDto);
        }

        [HttpPost("VerifyOtp")]
        public async Task<ResponseViewModel<bool>> VerifyOtpSecretAsync(VerifyOtpRequestViewModel requestViewModel)
        {
            var command = _mapper.Map<VerifyOtpCodeCommand>(requestViewModel);
            var isVerified = await _mediator.Send(command);
            if (!isVerified)
                return ResponseViewModel<bool>.ErrorResult("Invalid OTP code.", false, ErrorCode.InvalidCredentials);

            return ResponseViewModel<bool>.SuccessResult(true);
        }

            


        [HttpGet("GetAllUsers")]
        public async Task<ResponseViewModel<IEnumerable<UserViewModel>>> GetAllUser()
        {
            var user = await _mediator.Send(new GetAllUserQuery());
            var result = _mapper.Map<IEnumerable<UserViewModel>>(user);
            return ResponseViewModel<IEnumerable<UserViewModel>>.SuccessResult(result);
        }

        [HttpGet("GetUser")]
        public async Task<ResponseViewModel<UserViewModel>> GetUserByID(int Id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(Id));
            var result = _mapper.Map<UserViewModel>(user);
            return ResponseViewModel<UserViewModel>.SuccessResult(result);
        }

    }
}