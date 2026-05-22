using restaurant_management_system._2.Domain.Application.Interfaces;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace restaurant_management_system._2.Infrastructure.Repositories
{
    public class FileTableRepository : ITableRepository
    {
        private readonly FileStorage storage;

        public FileTableRepository(FileStorage storage)
        {
            this.storage = storage;
        }

        public List<Table> GetAll()
        {
            TableStorage db = storage.Load();
            return db.Tables;
        }

        public Table GetById(int id)
        {
            TableStorage db = storage.Load();
            return db.Tables.FirstOrDefault(t => t.Id == id);
        }

        public Table GetByNumber(int number)
        {
            TableStorage db = storage.Load();
            return db.Tables.FirstOrDefault(t => t.Number == number);
        }

        public void Save(Table table)
        {
            TableStorage db = storage.Load();

            //if (table.Id == 0)
            //{
            //    table.Id = db.NextId;
            //    db.NextId++;
            //}
        }
    }
}