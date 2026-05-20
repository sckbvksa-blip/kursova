using System.Collections.Generic;
using RecipesApp.Core.Domain;

namespace RecipesApp.Core.Interfaces
{
    public interface IRecipeService
    {
        List<Recipe> SearchRecipes(string query);
        List<Recipe> FilterByCategory(RecipeCategory category);
        void ToggleFavorite(string recipeId);
        void UpdateNotes(string recipeId, string newNotes);
    
        Recipe ScaleRecipe(Recipe original, double factor);
    }

    public interface IShoppingListService
    {
        ShoppingList CreateFromRecipe(Recipe recipe, string listName);
        void AddIngredientToList(string listId, Ingredient ingredient);
        void RemoveIngredientFromList(string listId, string ingredientName);
    }
}
