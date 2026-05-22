using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Service
{
    public class TableService
    {
        private readonly RestaurantDbContext db;

        public TableService(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<Table> GetAllTables()
        {
            return db.Tables
                .OrderBy(t => t.Number)
                .ToList();
        }

        public Table GetTableById(int tableId)
        {
            Table? table = db.Tables
                .FirstOrDefault(t => t.Id == tableId);

            if (table == null)
                throw new ArgumentException("Table not found.");

            return table;
        }

        public List<Table> GetFreeTables()
        {
            return db.Tables
                .Where(t => !t.IsOccupied && !t.IsReserved)
                .OrderBy(t => t.Number)
                .ToList();
        }
        public Table ReserveTable(int tableNumber)
        {
            Table? table = db.Tables.FirstOrDefault(t => t.Number == tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Occupied table cannot be reserved.");

            if (table.IsReserved)
                throw new ArgumentException("Table is already reserved.");

            table.IsReserved = true;

            db.SaveChanges();

            return table;
        }

        public Table OccupyTable(int tableNumber)
        {
            Table? table = db.Tables.FirstOrDefault(t => t.Number == tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Table is already occupied.");

            table.IsOccupied = true;
            table.IsReserved = false;

            db.SaveChanges();

            return table;
        }

        public Table FreeTable(int tableNumber)
        {
            Table? table = db.Tables.FirstOrDefault(t => t.Number == tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (!table.IsOccupied)
                throw new ArgumentException("Only occupied tables can be freed.");

            table.IsOccupied = false;

            db.SaveChanges();

            return table;
        }

        public Table CancelReservation(int tableNumber)
        {
            Table? table = db.Tables.FirstOrDefault(t => t.Number == tableNumber);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (!table.IsReserved)
                throw new ArgumentException("Table is not reserved.");

            if (table.IsOccupied)
                throw new ArgumentException("Cannot cancel reservation for occupied table.");

            table.IsReserved = false;

            db.SaveChanges();

            return table;
        }
    }
}