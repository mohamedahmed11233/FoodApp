using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel.Category
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
