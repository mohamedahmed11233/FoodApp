using Application.Dtos.Favorite;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Queries
{
    public class GetFavoritesByUserIdQuery : IRequest<List<FavoriteDto>>
    {
        public Guid UserId { get; set; }

        public GetFavoritesByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }

}
