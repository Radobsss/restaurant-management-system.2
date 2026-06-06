using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Application.Interface
{
    public interface IMenuItemRepository
    {
        List<MenuItem> GetAll();

        List<MenuItem> GetActive();

        MenuItem? GetById(int id);

        MenuItem? GetByName(string name);

        void Add(MenuItem menuItem);

        void Update(MenuItem menuItem);
    }
}