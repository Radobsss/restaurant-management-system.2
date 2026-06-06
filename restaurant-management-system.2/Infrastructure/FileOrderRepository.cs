using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Infrastructure
{
    public class FileOrderRepository : IOrderRepository
    {
        private readonly RestaurantDbContext db;

        public FileOrderRepository(RestaurantDbContext db)
        {
            this.db = db;
        }

        public List<Order> GetAll()
        {
            return db.Orders
                .Include(o => o.Table)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public Order? GetById(int id)
        {
            return db.Orders
                .Include(o => o.Table)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetByTableId(int tableId)
        {
            return db.Orders
                .Include(o => o.Table)
                .Where(o => o.TableId == tableId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public bool HasOrderForTableWithStatus(int tableId, OrderStatus status)
        {
            return db.Orders
                .Any(o => o.TableId == tableId && o.Status == status);
        }

        public void Add(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public void Update(Order order)
        {
            db.Orders.Update(order);
            db.SaveChanges();
        }
    }
}