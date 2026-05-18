using System;
using System.Text;
using RecipesApp.Core.DataAccess;
using RecipesApp.Core.Domain;
using RecipesApp.Core.Services;

namespace RecipesApp.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            JsonRepository<Recipe> recipeRepository = new JsonRepository<Recipe>("recipes.json");
            JsonRepository<ShoppingList> listRepository = new JsonRepository<ShoppingList>("shopping_lists.json");

            RecipeService recipeService = new RecipeService(recipeRepository);
            ShoppingListService listService = new ShoppingListService(listRepository);

            recipeService.OnFavoriteToggled += ShowFavoriteNotification;

            Menu menu = new Menu(recipeRepository, recipeService, listService);
            menu.Run();
        }

        static void ShowFavoriteNotification(Recipe recipe)
        {
            Console.WriteLine("\n[ПОДІЯ] Статус рецепта '" + recipe.Title + "' змінено!");
            Console.WriteLine("Натисніть Enter для продовження...");
            Console.ReadLine();
        }
    }
}
