using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommand : IRequest<string>
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        public RemoveFavoriteCommand(int userId, int recipeId)
        {
            UserId = userId;
            RecipeId = recipeId;
        }
    }
}
