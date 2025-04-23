using Application.Dtos.Favorite;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Favorite.Queries
{
    public class GetFavoritesByUserIdQuery : IRequest<List<FavoriteDto>>, IBaseRequest
    {
        public int UserId { get; set; }

        // Add a parameterized constructor to resolve the CS7036 error  
        public GetFavoritesByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }


}
