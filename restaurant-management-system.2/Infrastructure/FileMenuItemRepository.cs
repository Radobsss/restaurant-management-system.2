using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileMenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantDbContext db;

        public FileMenuItemRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<MenuItem> GetAll()
        {
            return db.MenuItems
                .Include(m => m.Category)
                .OrderBy(m => m.Name)
                .ToList();
        }

        public List<MenuItem> GetActive()
        {
            return db.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsActive)
                .OrderBy(m => m.Category!.Name)
                .ThenBy(m => m.Name)
                .ToList();
        }

        public MenuItem? GetById(int id)
        {
            return db.MenuItems
                .Include(m => m.Category)
                .FirstOrDefault(m => m.Id == id);
        }

        public MenuItem? GetByName(string name)
        {
            return db.MenuItems
                .Include(m => m.Category)
                .FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }

        public void Add(MenuItem menuItem)
        {
            db.MenuItems.Add(menuItem);
            db.SaveChanges();
        }

        public void Update(MenuItem menuItem)
        {
            db.MenuItems.Update(menuItem);
            db.SaveChanges();
        }
    }
}