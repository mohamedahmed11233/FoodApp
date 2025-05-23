﻿using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Presentation.ViewModel.Auth;
using AutoMapper;
using Application.Dtos.Auth;
using Domain.Enum.SharedEnums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Application.CQRS.Auth.Queries.OTP;
using Application.CQRS.Auth.Commands.OTP;
using Application.CQRS.Auth.Commands.ResetPassword;
using Application.CQRS.Auth.Commands.ForgetPassword;

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
            return ResponseViewModel<RegisterResponseDto>.ErrorResult(result.Message,  ErrorCode.FailedLogin);
        }

        [HttpGet("generate-otp-secret")]
        public async Task<ResponseViewModel<GenerateSecretKeyDTO>> GenerateOtpSecretAsync(int userId)
        {
            var secretKeyDto = await _mediator.Send(new GenerateSecretKeyQuery(userId));
            if (secretKeyDto == null)
                return ResponseViewModel<GenerateSecretKeyDTO>.ErrorResult("Failed to generate OTP secret key.",ErrorCode.NotFound);
            return ResponseViewModel<GenerateSecretKeyDTO>.SuccessResult(secretKeyDto);
        }

        [HttpPost("VerifyOtp")]
        public async Task<ResponseViewModel<bool>> VerifyOtpSecretAsync(VerifyOtpRequestViewModel requestViewModel)
        {
            var command =  _mapper.Map<VerifyOtpCodeCommand>(requestViewModel);
            var isVerified = await _mediator.Send(command);
            if (!isVerified)
                return ResponseViewModel<bool>.ErrorResult("Invalid OTP code.", ErrorCode.InvalidCredentials);

            return ResponseViewModel<bool>.SuccessResult(true);
        }


        [HttpPost("reset-password")]

        public async Task<ResponseViewModel<bool>> ResetPasswordAsync(ResetPasswordRequestVM requestViewModel)
        {
            var command = _mapper.Map<ResetPasswordCommand>(requestViewModel);
            var isSuccess = await _mediator.Send(command);
            if (!isSuccess)
                return ResponseViewModel<bool>.ErrorResult("Failed to reset password.", ErrorCode.InvalidCredentials);
            return ResponseViewModel<bool>.SuccessResult(true);
        }
        [HttpPost("forgot-password")]
        public async Task<ResponseViewModel<bool>> ForgotPassword(string Email)
        {
            var isSuccess = await _mediator.Send(new RequestResetPasswordCommand(Email));
            if (!isSuccess)
                return ResponseViewModel<bool>.ErrorResult("User with the given email was not found.", ErrorCode.UserNotFound);
            return ResponseViewModel<bool>.SuccessResult(true);
        }

     

    }
}