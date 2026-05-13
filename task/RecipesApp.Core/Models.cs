using System;
using System.Collections.Generic;

namespace RecipesApp.Core
{
    public enum RecipeCategory
    {
        Breakfast,
        MainCourse,
        Dessert,
        Snack,
        Drink
    }

    public enum MeasurementUnit
    {
        Gram,
        Kilogram,
        Milliliter,
        Liter,
        Piece,
        Teaspoon,
        Tablespoon
    }

    public struct Quantity
    {
        public double Value { get; set; }
        public MeasurementUnit Unit { get; set; }

        public Quantity(double value, MeasurementUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString()
        {
            return Value.ToString() + " " + Unit.ToString();
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public Quantity Amount { get; set; }

        public Ingredient(string name, Quantity amount)
        {
            Name = name;
            Amount = amount;
        }
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

    public class Recipe
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
            Ingredients = new List<Ingredient>();
            Steps = new List<RecipeStep>();
        }
    }

    public class ShoppingList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient> Items { get; set; }

        public ShoppingList(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Items = new List<Ingredient>();
        }
    }
}
