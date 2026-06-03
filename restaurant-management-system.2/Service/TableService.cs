using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Service
{
    public class TableService
    {
        private readonly ITableRepository tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public List<Table> GetAllTables()
        {
            return tableRepository.GetAll()
                .OrderBy(t => t.Number)
                .ToList();
        }

        public Table GetTableById(int tableId)
        {
            Table? table = tableRepository.GetById(tableId);

            if (table == null)
                throw new ArgumentException("Table not found.");

            return table;
        }

        public List<Table> GetFreeTables()
        {
            return tableRepository.GetAll()
                .Where(t => !t.IsOccupied && !t.IsReserved)
                .OrderBy(t => t.Number)
                .ToList();
        }

        public Table ReserveTable(int tableNumber)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Occupied table cannot be reserved.");

            if (table.IsReserved)
                throw new ArgumentException("Table is already reserved.");

            table.IsReserved = true;
            table.IsOccupied = false;

            tableRepository.Update(table);

            return table;
        }

        public Table OccupyTable(int tableNumber)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Table is already occupied.");

            table.IsOccupied = true;
            table.IsReserved = false;

            tableRepository.Update(table);

            return table;
        }

        public Table FreeTable(int tableNumber)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (!table.IsOccupied)
                throw new ArgumentException("Only occupied tables can be freed.");

            table.IsOccupied = false;
            table.IsReserved = false;

            tableRepository.Update(table);

            return table;
        }

        public Table CancelReservation(int tableNumber)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (!table.IsReserved)
                throw new ArgumentException("Table is not reserved.");

            if (table.IsOccupied)
                throw new ArgumentException("Cannot cancel reservation for occupied table.");

            table.IsReserved = false;

            tableRepository.Update(table);

            return table;
        }
    }
}