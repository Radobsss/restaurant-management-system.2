using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;
using restaurant_management_system._2.Infrastructure.Data;


namespace restaurant_management_system._2.Service
{
    public class MenuService
    {

        private readonly ICategoryRepository categoryRepository;
        private readonly IMenuItemRepository menuItemRepository;

        public MenuService(
            ICategoryRepository categoryRepository,
            IMenuItemRepository menuItemRepository)
        {
            this.categoryRepository = categoryRepository;
            this.menuItemRepository = menuItemRepository;
        }


        public Category AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.");

            string trimmedName = name.Trim();

            bool exists = db.Categories
                .Any(c => c.Name.ToLower() == trimmedName.ToLower());

            if (exists)
                throw new ArgumentException("Category with this name already exists.");

            Category category = new Category
            {
                Name = trimmedName
            };

            db.Categories.Add(category);
            db.SaveChanges();

            return category;
        }

        public List<Category> GetAllCategories()
        {
            return db.Categories
                .OrderBy(c => c.Name)
                .ToList();
        }

        public MenuItem AddMenuItem(
            string name,
            decimal price,
            MenuItemType type,
            int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Menu item name cannot be empty.");

            if (price <= 0)
                throw new ArgumentException("Price must be greater than 0.");

            Category? category = db.Categories
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
                throw new ArgumentException("Category not found.");

            string trimmedName = name.Trim();

            bool exists = db.MenuItems
                .Any(m => m.Name.ToLower() == trimmedName.ToLower());

            if (exists)
                throw new ArgumentException("Menu item with this name already exists.");

            MenuItem menuItem = new MenuItem
            {
                Name = trimmedName,
                Price = price,
                Type = type,
                CategoryId = categoryId,
                IsActive = true
            };

            db.MenuItems.Add(menuItem);
            db.SaveChanges();

            return menuItem;
        }

        public MenuItem ChangeMenuItemPrice(int menuItemId, decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than 0.");

            MenuItem? menuItem = db.MenuItems
                .FirstOrDefault(m => m.Id == menuItemId);

            if (menuItem == null)
                throw new ArgumentException("Menu item not found.");

            menuItem.Price = newPrice;

            db.MenuItems.Update(menuItem);
            db.SaveChanges();

            return menuItem;
        }

        public MenuItem HideMenuItem(int menuItemId)
        {
            MenuItem? menuItem = db.MenuItems
                .FirstOrDefault(m => m.Id == menuItemId);

            if (menuItem == null)
                throw new ArgumentException("Menu item not found.");

            if (!menuItem.IsActive)
                throw new ArgumentException("Menu item is already hidden.");

            menuItem.IsActive = false;

            db.MenuItems.Update(menuItem);
            db.SaveChanges();

            return menuItem;
        }

        public List<MenuItem> GetActiveMenuItems()
        {
            return db.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsActive)
                .OrderBy(m => m.Category!.Name)
                .ThenBy(m => m.Name)
                .ToList();
        }

        public Dictionary<string, List<MenuItem>> GetActiveItemsGroupedByCategory()
        {
            return db.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsActive)
                .OrderBy(m => m.Category!.Name)
                .ThenBy(m => m.Name)
                .AsEnumerable()
                .GroupBy(m => m.Category != null ? m.Category.Name : "No category")
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList());
        }
    }
}