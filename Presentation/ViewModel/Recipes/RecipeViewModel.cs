﻿namespace Presentation.ViewModel.Recipes
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsFavorite { get; set; }


    }
}
