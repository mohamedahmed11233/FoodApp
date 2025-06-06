﻿namespace Presentation.ViewModel.Recipes
{
    public class AddRecipeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public decimal Discount { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsFavorite { get; set; }
    }
}
