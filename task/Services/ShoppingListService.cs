using System.Collections.Generic;
using RecipesApp.Core.Domain;
using RecipesApp.Core.Interfaces;

namespace RecipesApp.Core.Services
{
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
