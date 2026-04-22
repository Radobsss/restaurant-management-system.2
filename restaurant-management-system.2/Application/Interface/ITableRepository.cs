using RestaurantProject.Domain.Entities;
using System.Collections.Generic;

namespace RestaurantProject.Application.Interfaces
{
    public interface ITableRepository
    {
        List<Table> GetAll();
        Table GetById(int id);
        Table GetByNumber(int number);
        void Save(Table table);
    }
}