using System;
using System.Collections.Generic;

namespace RecipesApp.Core.Domain
{
    public class ShoppingList : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient> Items { get; set; }

        public ShoppingList()
        {
            Id = Guid.NewGuid().ToString();
            Name = "New Shopping List";
            Items = new List<Ingredient>();
        }
    }
}
