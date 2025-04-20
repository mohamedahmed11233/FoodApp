using Domain.Enum.SharedEnums;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel
{
    public class UserViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; } 
        public string Username { get; set; }
        [Required]
        public string Email { get; set; } 
        public Roles Role { get; set; }

    }
}

