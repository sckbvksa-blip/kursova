using System;
using System.IO;

namespace RecipesApp.Core.DataAccess
{
    public class JsonRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly string _filePath;
        private List<T> _items;

        public JsonRepository(string fileName)
        {
            string directory = "Data";
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _filePath = Path.Combine(directory, fileName);
            
            _items = LoadFromFile();
        }

        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                List<T> loadedItems = JsonSerializer.Deserialize<List<T>>(json);
                
                if (loadedItems == null)
                {
                    return new List<T>();
                }
                
                return loadedItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при читанні файлу " + _filePath + ": " + ex.Message);
                return new List<T>();
            }
        }

        public void Save()
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                string json = JsonSerializer.Serialize(_items, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при збереженні файлу " + _filePath + ": " + ex.Message);
            }
        }

        public List<T> GetAll()
        {
            return _items;
        }

        public T GetById(string id)
        {
            foreach (T item in _items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return default(T);
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Update(T item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == item.Id)
                {
                    _items[i] = item;
                    break;
                }
            }
        }

        public void Delete(string id)
        {
            T itemToRemove = GetById(id);
            if (itemToRemove != null)
            {
                _items.Remove(itemToRemove);
            }
        }
    }
}
