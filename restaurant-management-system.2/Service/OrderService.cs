using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;
using restaurant_management_system._2.Infrastructure.Data;

namespace restaurant_management_system._2.Service
{
    public class OrderService
    {
        private readonly RestaurantDbContext db;

        public OrderService(RestaurantDbContext db)
        {
            this.db = db;
        }

        public Order CreateOrder(int tableId)
        {
            Table? table = db.Tables.FirstOrDefault(t => t.Id == tableId);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (table.IsOccupied)
                throw new ArgumentException("Table is already occupied.");

            Order order = new Order
            {
                TableId = tableId,
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Open
            };

            table.IsOccupied = true;

            db.Orders.Add(order);
            db.SaveChanges();

            return order;
        }

        public void AddItemToOrder(int orderId, int menuItemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");

            Order? order = db.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            MenuItem? item = db.MenuItems.FirstOrDefault(m => m.Id == menuItemId);

            if (item == null)
                throw new ArgumentException("Menu item not found.");

            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                MenuItemId = menuItemId,
                Quantity = quantity,
                UnitPrice = item.Price,
                IsServed = false
            };

            db.OrderItems.Add(orderItem);
            db.SaveChanges();
        }

        public void RemoveItemFromOrder(int orderItemId)
        {
            OrderItem? orderItem = db.OrderItems.FirstOrDefault(o => o.Id == orderItemId);

            if (orderItem == null)
                throw new ArgumentException("Order item not found.");

            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
        }

        public void ChangeOrderStatus(int orderId, OrderStatus status)
        {
            Order? order = db.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            order.Status = status;

            db.SaveChanges();
        }

        public decimal CalculateTotal(int orderId)
        {
            List<OrderItem> items = db.OrderItems
                .Where(o => o.OrderId == orderId)
                .ToList();

            decimal total = 0;

            foreach (OrderItem item in items)
            {
                total += item.Quantity * item.UnitPrice;
            }

            return total;
        }

        public void CloseOrder(int orderId)
        {
            Order? order = db.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            Table? table = db.Tables.FirstOrDefault(t => t.Id == order.TableId);

            if (table != null)
            {
                table.IsOccupied = false;
            }

            order.Status = OrderStatus.Closed;

            db.SaveChanges();
        }
    }
}