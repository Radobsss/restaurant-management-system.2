using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Application.Interface
{
    public interface ITableRepository
    {
        List<Table> GetAll();

        Table? GetById(int id);

        Table? GetByNumber(int number);

        void Add(Table table);

        void Update(Table table);
    }
}