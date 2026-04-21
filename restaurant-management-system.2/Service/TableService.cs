using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Service
{
    public class TableService
    {
        private readonly List<Table> tables = new List<Table>();

        public Table AddTable(int capacity, string location)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");

            if (location == null || location == "")
            {
                throw new ArgumentException("Location cannot be empty.");
            }

            Table table = new Table
            {
                Id = tables.Count + 1,
                Capacity = capacity,
                Location = location,
                IsOccupied = false
            };

            tables.Add(table);
            return table;
        }
    }
}
