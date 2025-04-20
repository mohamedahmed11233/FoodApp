using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Category
{
   public class CategoryDto
   {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public ICollection<RecipeDto> Recipes { get; set; } = new HashSet<RecipeDto>();
   }
}
