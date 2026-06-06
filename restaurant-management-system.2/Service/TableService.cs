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

        public Table GetTableByNumber(int tableNumber)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

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

        public List<Table> GetReservableTables()
        {
            return tableRepository.GetAll()
                .Where(t => !t.IsOccupied && !t.IsReserved)
                .OrderBy(t => t.Number)
                .ToList();
        }

        public List<Table> GetTablesAvailableForOccupy()
        {
            return tableRepository.GetAll()
                .Where(t => !t.IsOccupied)
                .OrderBy(t => t.Number)
                .ToList();
        }

        public List<Table> GetOccupiedTables()
        {
            return tableRepository.GetAll()
                .Where(t => t.IsOccupied)
                .OrderBy(t => t.Number)
                .ToList();
        }

        public Table ReserveTable(int tableNumber, string reservedBy)
        {
            if (string.IsNullOrWhiteSpace(reservedBy))
                throw new ArgumentException("Reservation name cannot be empty.");

            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Occupied table cannot be reserved.");

            if (table.IsReserved)
                throw new ArgumentException("Table is already reserved.");

            table.IsReserved = true;
            table.IsOccupied = false;
            table.ReservedBy = reservedBy.Trim();

            tableRepository.Update(table);

            return table;
        }

        public Table OccupyTable(int tableNumber, string? reservationName = null)
        {
            Table? table = tableRepository.GetByNumber(tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Table is already occupied.");

            if (table.IsReserved)
            {
                if (string.IsNullOrWhiteSpace(reservationName))
                    throw new ArgumentException("This table is reserved. Reservation name is required.");

                bool isCorrectName = string.Equals(
                    table.ReservedBy?.Trim(),
                    reservationName.Trim(),
                    StringComparison.OrdinalIgnoreCase);

                if (!isCorrectName)
                    throw new ArgumentException("Reservation name is not correct.");
            }

            table.IsOccupied = true;
            table.IsReserved = false;
            table.ReservedBy = null;

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
            table.ReservedBy = null;

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
            table.ReservedBy = null;

            tableRepository.Update(table);

            return table;
        }
    }
}