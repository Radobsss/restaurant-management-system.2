using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Application.Interface
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();

        Category? GetById(int id);

        Category? GetByName(string name);

        void Add(Category category);

        void Update(Category category);
    }
}