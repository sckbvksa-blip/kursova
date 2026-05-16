using System;
using System.Collections.Generic;

namespace RecipesApp.Core
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

    public class ShoppingListService : IShoppingListService
    {
        private readonly IRepository<ShoppingList> _listRepository;

        public ShoppingListService(IRepository<ShoppingList> listRepository)
        {
            _listRepository = listRepository;
        }

        public ShoppingList CreateFromRecipe(Recipe recipe, string listName)
        {
            ShoppingList newList = new ShoppingList();
            newList.Name = listName;

            foreach (Ingredient ing in recipe.Ingredients)
            {
                Quantity quantity = new Quantity(ing.Amount.Value, ing.Amount.Unit);
                Ingredient item = new Ingredient(ing.Name, quantity);
                newList.Items.Add(item);
            }

            _listRepository.Add(newList);
            _listRepository.Save();
            
            return newList;
        }

        public void AddIngredientToList(string listId, Ingredient ingredient)
        {
            ShoppingList list = _listRepository.GetById(listId);
            if (list == null)
            {
                throw new KeyNotFoundException("Список покупок не знайдено.");
            }

            list.Items.Add(ingredient);
            _listRepository.Update(list);
            _listRepository.Save();
        }

        public void RemoveIngredientFromList(string listId, string ingredientName)
        {
            ShoppingList list = _listRepository.GetById(listId);
            if (list == null)
            {
                throw new KeyNotFoundException("Список покупок не знайдено.");
            }

            Ingredient itemToRemove = null;
            foreach (Ingredient item in list.Items)
            {
                if (item.Name.ToLower() == ingredientName.ToLower())
                {
                    itemToRemove = item;
                    break;
                }
            }

            if (itemToRemove != null)
            {
                list.Items.Remove(itemToRemove);
                _listRepository.Update(list);
                _listRepository.Save();
            }
        }
    }
}