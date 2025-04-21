using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Commands.AddFavorite
{
    public class AddFavoriteCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }

}
