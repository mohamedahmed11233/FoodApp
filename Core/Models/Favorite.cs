using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Favorite:BaseEntity
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Recipe Recipe { get; set; }
        public User User { get; set; }
    }
}
