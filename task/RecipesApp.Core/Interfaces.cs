using System.Collections.Generic;

namespace RecipesApp.Core
{
    public interface IRepository<T> where T : IEntity
    {
        List<T> GetAll();
        T GetById(string id);
        void Add(T item);
        void Update(T item);
        void Delete(string id);
        void Save();
    }

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