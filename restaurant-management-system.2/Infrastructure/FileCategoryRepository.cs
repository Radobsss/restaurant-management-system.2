using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileCategoryRepository : ICategoryRepository
    {
        private readonly RestaurantDbContext db;

        public FileCategoryRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<Category> GetAll()
        {
            return db.Categories
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Category? GetById(int id)
        {
            return db.Categories
                .FirstOrDefault(c => c.Id == id);
        }

        public Category? GetByName(string name)
        {
            return db.Categories
                .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
        }

        public void Add(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public void Update(Category category)
        {
            db.Categories.Update(category);
            db.SaveChanges();
        }
    }
}