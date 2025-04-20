using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel.Category
{
    public class UpdateCategoryViewModel
    {
        [ReadOnly(true)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
