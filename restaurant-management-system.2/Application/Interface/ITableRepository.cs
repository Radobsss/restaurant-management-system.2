using restaurant_management_system._2.Domain.Entities;
using System.Collections.Generic;

namespace restaurant_management_system._2.Domain.Application.Interfaces
{
    public interface ITableRepository
    {
        List<Table> GetAll();
        Table GetById(int id);
        Table GetByNumber(int number);
        void Save(Table table);
    }
}