using System.Collections.Generic;
using RecipesApp.Core.Domain;

namespace RecipesApp.Core.Interfaces
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
}
