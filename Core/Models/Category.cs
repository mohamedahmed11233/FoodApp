using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string ?Description { get; set; }
       
        public ICollection<Recipe> Recipe { get; set; } = new List<Recipe>();
        public int RecipeId { get; set; }

    }

}