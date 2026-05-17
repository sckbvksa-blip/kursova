using System;
using System.Collections.Generic;
using RecipesApp.Core.Domain;
using RecipesApp.Core.Interfaces;

namespace RecipesApp.Core.Services
{
    public delegate void RecipeEventHandler(Recipe recipe);

    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;

        public event RecipeEventHandler OnFavoriteToggled;

        public RecipeService(IRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public List<Recipe> SearchRecipes(string query)
        {
            List<Recipe> allRecipes = _recipeRepository.GetAll();
            List<Recipe> result = new List<Recipe>();
            string lowerQuery = query.ToLower();

            foreach (Recipe recipe in allRecipes)
            {
                if (recipe.Title.ToLower().Contains(lowerQuery) || recipe.Description.ToLower().Contains(lowerQuery))
                {
                    result.Add(recipe);
                }
            }
            return result;
        }

        public List<Recipe> FilterByCategory(RecipeCategory category)
        {
            List<Recipe> allRecipes = _recipeRepository.GetAll();
            List<Recipe> result = new List<Recipe>();

            foreach (Recipe recipe in allRecipes)
            {
                if (recipe.Category == category)
                {
                    result.Add(recipe);
                }
            }
            return result;
        }

        public void ToggleFavorite(string recipeId)
        {
            Recipe recipe = _recipeRepository.GetById(recipeId);
            
            if (recipe == null)
            {
                throw new KeyNotFoundException("Рецепт з ID " + recipeId + " не знайдено.");
            }

            recipe.IsFavorite = !recipe.IsFavorite;
            _recipeRepository.Update(recipe);
            _recipeRepository.Save();

            if (OnFavoriteToggled != null)
            {
                OnFavoriteToggled(recipe);
            }
        }

        public void UpdateNotes(string recipeId, string newNotes)
        {
            Recipe recipe = _recipeRepository.GetById(recipeId);
            if (recipe == null)
            {
                throw new KeyNotFoundException("Рецепт з ID " + recipeId + " не знайдено.");
            }

            recipe.Notes = newNotes;
            _recipeRepository.Update(recipe);
            _recipeRepository.Save();
        }

        public Recipe ScaleRecipe(Recipe original, double factor)
        {
            if (factor <= 0)
            {
                throw new ArgumentException("Коефіцієнт масштабування має бути більшим за нуль.");
            }

            Recipe scaledRecipe = new Recipe();
            scaledRecipe.Title = original.Title + " (Масштабований x" + factor + ")";
            scaledRecipe.Description = original.Description;
            scaledRecipe.Category = original.Category;
            scaledRecipe.PreparationTimeMinutes = original.PreparationTimeMinutes;
            scaledRecipe.DifficultyLevel = original.DifficultyLevel;
            scaledRecipe.Notes = original.Notes;

            foreach (Ingredient ing in original.Ingredients)
            {
                Quantity newQuantity = new Quantity(ing.Amount.Value * factor, ing.Amount.Unit);
                Ingredient newIngredient = new Ingredient(ing.Name, newQuantity);
                scaledRecipe.Ingredients.Add(newIngredient);
            }

            foreach (RecipeStep step in original.Steps)
            {
                scaledRecipe.Steps.Add(new RecipeStep(step.StepNumber, step.Description));
            }

            return scaledRecipe;
        }
    }
}
