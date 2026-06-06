using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileOrderItemRepository : IOrderItemRepository
    {
        private readonly RestaurantDbContext db;

        public FileOrderItemRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<OrderItem> GetByOrderId(int orderId)
        {
            return db.OrderItems
                .Include(oi => oi.MenuItem)
                .Where(oi => oi.OrderId == orderId)
                .ToList();
        }

        public OrderItem? GetById(int id)
        {
            return db.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.MenuItem)
                .FirstOrDefault(oi => oi.Id == id);
        }

        public OrderItem? GetByOrderAndMenuItem(int orderId, int menuItemId)
        {
            return db.OrderItems
                .Include(oi => oi.MenuItem)
                .FirstOrDefault(oi =>
                    oi.OrderId == orderId &&
                    oi.MenuItemId == menuItemId);
        }

        public void Add(OrderItem orderItem)
        {
            db.OrderItems.Add(orderItem);
            db.SaveChanges();
        }

        public void Update(OrderItem orderItem)
        {
            db.OrderItems.Update(orderItem);
            db.SaveChanges();
        }

        public void Delete(OrderItem orderItem)
        {
            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
        }
    }
}