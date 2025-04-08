using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Recipe> recipes { get; set; } = new List<Recipe>();


    }
    
}