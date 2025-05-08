using Application.CQRS.Recipe.Commands;
using Application.Dtos.Recipe;
using AutoMapper;
using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using Presentation.ViewModel.RabbitMQ;
using Presentation.ViewModel.Recipes;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICapPublisher _cap;
        private readonly RabbitMQPublisherService _rabbitMQ;
        private readonly IMediator _mediator;

        public RabbitMQController(IMapper mapper ,ICapPublisher cap, RabbitMQPublisherService rabbitMQ , IMediator mediator)
        {
            this._mapper = mapper;
            this._cap = cap;
            this._rabbitMQ = rabbitMQ;
            this._mediator = mediator;
        }
        [HttpPost]
        public Task<ResponseViewModel<string>> PublishMessage(string message)
        {
            _rabbitMQ.PublishMessage(message);
            return Task.FromResult(new ResponseViewModel<string>
            (
                success: true,
                data: "Message published successfully"
            ));
        }



        [HttpPost("AddRecipeWithRabbitMQ")]
        public async Task<ResponseViewModel<AddRecipeViewModel>> AddRecipeWithRabbitMq(string message, AddRecipeViewModel model)
        {
            var recipeDto = _mapper.Map<AddRecipeDto>(model);

            var result = await _mediator.Send(new AddRecipeCommand(recipeDto));
            if (result is null)
            {
                return new ResponseViewModel<AddRecipeViewModel>(
                    success: false,
                    message: "Failed to add recipe",
                    data: null,
                    errorCode: Domain.Enum.SharedEnums.ErrorCode.InvalidRecipeData
                );
            }
            var recipeMessage = new AddRecipeMessage
            {
                Date = DateTime.Now,
                Action = "Add Recipe",
                CategoryName = model.Category,
                RecipeName = model.Name,
                Type = nameof(AddRecipeMessage),
            };
            var Message = Newtonsoft.Json.JsonConvert.SerializeObject(recipeMessage);
            _rabbitMQ.PublishMessage(Message);
            var recipeViewModel = _mapper.Map<AddRecipeViewModel>(result);
            return new ResponseViewModel<AddRecipeViewModel>
            (
                success: true,
                message: "Recipe Added Successfully",
                data: model
            );
        }
        [HttpPost("PublishMeesageUsingCAP")]
        public async Task PublishMessage()
        {

                     _cap.Publish("cap" , new {Id =1  , Name = "Noha" });
        }
    }
}
