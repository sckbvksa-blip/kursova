using System;
using System.Collections.Generic;
using RecipesApp.Core.Domain;
using RecipesApp.Core.Interfaces;

namespace RecipesApp.ConsoleUI
{
    public class Menu
    {
        private readonly IRepository<Recipe> _recipeRepo;
        private readonly IRecipeService _recipeService;
        private readonly IShoppingListService _listService;

        public Menu(IRepository<Recipe> recipeRepo, IRecipeService recipeService, IShoppingListService listService)
        {
            _recipeRepo = recipeRepo;
            _recipeService = recipeService;
            _listService = listService;
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Printer.PrintHeader("Головне меню");
                Console.WriteLine("1. Показати всі рецепти");
                Console.WriteLine("2. Додати новий рецепт");
                Console.WriteLine("3. Знайти рецепт");
                Console.WriteLine("4. Масштабувати рецепт (Варіант 5)");
                Console.WriteLine("5. Вихід");
                Console.WriteLine();

                int choice = InputHandler.GetInt("Оберіть пункт меню: ");

                switch (choice)
                {
                    case 1:
                        ShowAllRecipes();
                        break;
                    case 2:
                        AddNewRecipe();
                        break;
                    case 3:
                        SearchRecipe();
                        break;
                    case 4:
                        ScaleRecipeMenu();
                        break;
                    case 5:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть Enter.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void ShowAllRecipes()
        {
            Printer.PrintHeader("Список рецептів");
            List<Recipe> recipes = _recipeRepo.GetAll();

            if (recipes.Count == 0)
            {
                Console.WriteLine("Рецептів поки немає.");
            }
            else
            {
                foreach (Recipe r in recipes)
                {
                    Printer.PrintRecipeShort(r);
                }
            }

            Console.WriteLine("\nНатисніть Enter для повернення...");
            Console.ReadLine();
        }

        private void AddNewRecipe()
        {
            Printer.PrintHeader("Додавання нового рецепта");
            
            Recipe newRecipe = new Recipe();
            newRecipe.Title = InputHandler.GetString("Введіть назву рецепта: ");
            newRecipe.Description = InputHandler.GetString("Введіть опис: ");
            newRecipe.PreparationTimeMinutes = InputHandler.GetInt("Час приготування (хв): ");
            
            Console.WriteLine("\n--- Додавання інгредієнта ---");
            string ingName = InputHandler.GetString("Назва інгредієнта: ");
            double ingAmount = InputHandler.GetDouble("Кількість (число): ");
            
            Ingredient ingredient = new Ingredient(ingName, new Quantity(ingAmount, MeasurementUnit.Gram));
            newRecipe.Ingredients.Add(ingredient);

            _recipeRepo.Add(newRecipe);
            _recipeRepo.Save();

            Console.WriteLine("\nРецепт успішно додано та збережено!");
            Console.ReadLine();
        }

        private void SearchRecipe()
        {
            Printer.PrintHeader("Пошук рецепта");
            string query = InputHandler.GetString("Введіть слово для пошуку: ");

            List<Recipe> results = _recipeService.SearchRecipes(query);


            if (results.Count == 0)
            {
                Console.WriteLine("Нічого не знайдено.");
            }
            else
            {
                Console.WriteLine("Знайдено рецептів: " + results.Count);
                foreach (Recipe r in results)
                {
                    Printer.PrintRecipeShort(r);
                }
            }
            Console.ReadLine();
        }

        private void ScaleRecipeMenu()
        {
            Printer.PrintHeader("Масштабування порцій (Варіант 5)");
            
            string id = InputHandler.GetString("Введіть ID рецепта: ");
            Recipe original = _recipeRepo.GetById(id);

            if (original == null)
            {
                Console.WriteLine("Рецепт з таким ID не знайдено.");
                Console.ReadLine();
                return;
            }

            double factor = InputHandler.GetDouble("Введіть коефіцієнт (напр. 2 для подвійної порції, 0,5 для половини): ");

            try
            {
                Recipe scaled = _recipeService.ScaleRecipe(original, factor);
                Printer.PrintRecipeDetails(scaled);
                
                Console.WriteLine("Масштабування виконано успішно. Цей рецепт існує лише в пам'яті як розрахунок.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
            
            Console.ReadLine();
        }
    }
}
