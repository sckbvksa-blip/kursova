using System;

namespace RecipesApp.ConsoleUI
{
    public static class InputHandler
    {
        public static string GetString(string prompt)
        {
            string input = "";
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                
                if (!string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                Console.WriteLine("Помилка: поле не може бути порожнім. Спробуйте ще раз.");
            }
            return input;
        }

        public static int GetInt(string prompt)
        {
            int result = 0;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out result))
                {
                    break;
                }
                Console.WriteLine("Помилка: введіть коректне ціле число.");
            }
            return result;
        }

        public static double GetDouble(string prompt)
        {
            double result = 0;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                input = input.Replace(".", ",");

                if (double.TryParse(input, out result) && result > 0)
                {
                    break;
                }
                Console.WriteLine("Помилка: введіть коректне число, більше за нуль.");
            }
            return result;
        }
    }
}
