using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileTableRepository : ITableRepository
    {
        private readonly RestaurantDbContext db;

        public FileTableRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<Table> GetAll()
        {
            return db.Tables
                .OrderBy(t => t.Number)
                .ToList();
        }

        public Table? GetById(int id)
        {
            return db.Tables
                .FirstOrDefault(t => t.Id == id);
        }

        public Table? GetByNumber(int number)
        {
            return db.Tables
                .FirstOrDefault(t => t.Number == number);
        }

        public void Add(Table table)
        {
            db.Tables.Add(table);
            db.SaveChanges();
        }

        public void Update(Table table)
        {
            db.Tables.Update(table);
            db.SaveChanges();
        }
    }
}