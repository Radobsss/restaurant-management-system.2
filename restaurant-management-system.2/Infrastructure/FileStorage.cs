using System.IO;
using System.Text.Json;

namespace RestaurantProject.Infrastucture
{
    public class FileStorage
    {
        private readonly string path;

        public FileStorage(string path)
        {
            this.path = path;
        }

        public TableStorage Load()
        {
            if (!File.Exists(path))
            {
                return new TableStorage();
            }

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new TableStorage();
            }

            TableStorage storage = JsonSerializer.Deserialize<TableStorage>(json);

            if (storage == null)
            {
                return new TableStorage();
            }

            return storage;
        }

        public void Save(TableStorage storage)
        {
            string json = JsonSerializer.Serialize(storage, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }
    }
}