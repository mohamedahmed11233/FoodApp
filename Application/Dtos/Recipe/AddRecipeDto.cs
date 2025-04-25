using Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Recipe
{
    public class AddRecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public decimal Discount { get; set; }
        public string Category { get; set; } 
        public UserDto User { get; set; }
        public bool IsFavorite { get; set; }
    }
}
