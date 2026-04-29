using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Service
{
    public class TableService
    {
        private readonly List<Table> tables = new List<Table>();
        public Table AddTable(int number, int capacity, string location)
        {
            if (number <= 0)
                throw new ArgumentException("Table number must be greater than 0.");

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be empty.");

            if (tables.Any(t => t.Number == number))
                throw new ArgumentException("Table with this number already exists.");

            Table table = new Table
            {
                Id = tables.Count + 1,
                Number = number,
                Capacity = capacity,
                Location = location,
                IsOccupied = false
            };

            tables.Add(table);

            return table;
        }

        public Table GetTableById(int tableId)
        {
            Table table = tables.FirstOrDefault(t => t.Id == tableId);

            if (table == null)
                throw new ArgumentException("Table not found.");

            return table;
        }

        public List<Table> GetAllTables()
        {
            return tables;
        }
    }
}