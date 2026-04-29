using restaurant_management_system._2.Infrastructure;
using System.IO;
using System.Text.Json;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileStorage
    {
        private readonly string path = "data.json";

        public TableStorage Load()
        {
            if (!File.Exists(path))
                return new TableStorage();

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<TableStorage>(json);
        }

        public void Save(TableStorage data)
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }
    }
}