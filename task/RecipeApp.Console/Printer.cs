using System;
using System.Collections.Generic;
using RecipesApp.Core.Domain;

namespace RecipesApp.ConsoleUI
{
    public static class Printer
    {
        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("   " + title);
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

        public static void PrintRecipeShort(Recipe recipe)
        {
            string favStar = recipe.IsFavorite ? "[★]" : "[ ]";
            Console.WriteLine(favStar + " ID: " + recipe.Id + " | " + recipe.Title + " (" + recipe.Category + ")");
        }

        public static void PrintRecipeDetails(Recipe recipe)
        {
            PrintHeader("Рецепт: " + recipe.Title);
            Console.WriteLine("Категорія: " + recipe.Category);
            Console.WriteLine("Час приготування: " + recipe.PreparationTimeMinutes + " хв");
            Console.WriteLine("Складність: " + recipe.DifficultyLevel + "/5");
            Console.WriteLine("Улюблений: " + (recipe.IsFavorite ? "Так." : "Ні."));
            Console.WriteLine("Опис: " + recipe.Description);
            Console.WriteLine("Нотатки: " + recipe.Notes);
            
            Console.WriteLine("\n--- Інгредієнти ---");
            foreach (Ingredient ing in recipe.Ingredients)
            {
                Console.WriteLine("- " + ing.Name + ": " + ing.Amount.ToString());
            }

            Console.WriteLine("\n--- Кроки приготування ---");
            foreach (RecipeStep step in recipe.Steps)
            {
                Console.WriteLine(step.StepNumber + ". " + step.Description);
            }
            Console.WriteLine("========================================\n");
        }

        public static void PrintShoppingList(ShoppingList list)
        {
            PrintHeader("Список покупок: " + list.Name);
            if (list.Items.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            foreach (Ingredient item in list.Items)
            {
                Console.WriteLine("[ ] " + item.Name + " - " + item.Amount.ToString());
            }
            Console.WriteLine("========================================\n");
        }
    }
}
