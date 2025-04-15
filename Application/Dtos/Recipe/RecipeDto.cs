using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } 
        public decimal Discount { get; set; }
        public string Category { get; set; } 
    }
}
