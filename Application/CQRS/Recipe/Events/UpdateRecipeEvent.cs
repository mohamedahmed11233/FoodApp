using Application.Dtos.Recipe;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Recipe.Events
{
    public sealed record UpdateRecipeEvent(UpdateRecipeDto model) :INotification;
   
}
