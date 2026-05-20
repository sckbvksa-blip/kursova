using System.Collections.Generic;
using System.Linq;
using RecipesApp.Core.Domain;
using RecipesApp.Core.Interfaces;

namespace RecipesApp.Tests
{
    public class FakeRepository<T> : IRepository<T> where T : IEntity
    {
        public List<T> Items { get; set; } = new List<T>();

        public void Add(T item) => Items.Add(item);
        
        public void Delete(string id) => Items.RemoveAll(x => x.Id == id);
        
        public List<T> GetAll() => Items;
        
        public T GetById(string id) => Items.FirstOrDefault(x => x.Id == id);
        
        public void Update(T item)
        {
            int index = Items.FindIndex(x => x.Id == item.Id);
            if (index != -1)
            {
                Items[index] = item;
            }
        }
        
        public void Save() { } 
    }
}