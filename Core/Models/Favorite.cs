using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Favorite:BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Recipe Recipe { get; set; }
        public User User { get; set; }
    }
}
