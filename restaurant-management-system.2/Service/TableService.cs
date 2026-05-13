using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure;
using restaurant_management_system._2.Infrastructure.Repositories;
using System;
using System.Collections.Generic;

namespace restaurant_management_system._2.Service
{
    public class TableService
    {
        private readonly FileTableRepository tableRepository;

        public TableService(FileTableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public Table AddTable(int number, int capacity, string location)
        {
            if (number <= 0)
                throw new ArgumentException("Table number must be greater than 0.");

            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be empty.");

            Table existingTable = tableRepository.GetByNumber(number);

            if (existingTable != null)
                throw new ArgumentException("Table with this number already exists.");

            Table table = new Table
            {
                Number = number,
                Capacity = capacity,
                Location = location,
                IsOccupied = false
            };

            tableRepository.Save(table);

            return table;
        }

        public Table GetTableById(int tableId)
        {
            Table table = tableRepository.GetById(tableId);

            if (table == null)
                throw new ArgumentException("Table not found.");

            return table;
        }

        public List<Table> GetAllTables()
        {
            return tableRepository.GetAll();
        }
    }
}