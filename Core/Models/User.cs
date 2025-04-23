using Domain.Enum.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Username { get; set; } 
        public string Email { get; set; } 
        public string Password { get; set; }

        public string? OtpSekretKey { get; set; }=string.Empty;
        public string? ResetCode { get; set; } = string.Empty;  // New field for reset code

        public ICollection<Recipe>? Recipes { get; set; } = new HashSet<Recipe>();
        public Roles Role { get; set; } = Roles.User;
        public ICollection<Favorite>? Favorites { get; set; } = new HashSet<Favorite>();
    }

}
