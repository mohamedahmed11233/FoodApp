using Application.CQRS.Auth.Commend.AssignFeatures;
using Application.CQRS.Auth.Commend.AssignRoleToUser;
using Application.CQRS.Auth.Queries.UserDataOperation;
using AutoMapper;
using Domain.Enum.SharedEnums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FeaturesController(IMediator mediator , IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost("AssignRoleFeatures")]
        public async Task<ResponseViewModel<bool>> AssignRoleFeatures(AssignRoleFeaturesCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)  return ResponseViewModel<bool>.SuccessResult(true);

            return ResponseViewModel<bool>.ErrorResult("Failed to assign features", false);
        }

        [HttpPost("AssignRole")]
        public async Task<ResponseViewModel<bool>> AssignRole(AssignRoleToUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
                return ResponseViewModel<bool>.SuccessResult(true);

            return ResponseViewModel<bool>.ErrorResult("User not found", false, ErrorCode.UserNotFound);
        }
    }
}
