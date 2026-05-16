using System;
using System.Collections.Generic;

namespace RecipesApp.Core.Domain
{
    public interface IEntity
    {
        string Id { get; set; }
    }

    public class RecipeStep
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }

        public RecipeStep(int stepNumber, string description)
        {
            StepNumber = stepNumber;
            Description = description;
        }
    }

    public class Recipe : IEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public RecipeCategory Category { get; set; }
        public int PreparationTimeMinutes { get; set; }
        public int DifficultyLevel { get; set; }
        public bool IsFavorite { get; set; }
        public string Notes { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public List<RecipeStep> Steps { get; set; }

        public Recipe()
        {
            Id = Guid.NewGuid().ToString();
            Title = string.Empty;
            Description = string.Empty;
            Notes = string.Empty;
            Ingredients = new List<Ingredient>();
            Steps = new List<RecipeStep>();
        }
    }
}
