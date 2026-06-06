using System;
using System.Collections.Generic;
using System.Linq;
using restaurant_management_system._2.Application.Interface;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Service
{
    public class OrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly ITableRepository tableRepository;
        private readonly IMenuItemRepository menuItemRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            ITableRepository tableRepository,
            IMenuItemRepository menuItemRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.tableRepository = tableRepository;
            this.menuItemRepository = menuItemRepository;
        }

        public Order CreateOrder(int tableId)
        {
            Table? table = tableRepository.GetById(tableId);

            if (table == null)
                throw new ArgumentException("Table not found.");

            if (!table.IsOccupied)
                throw new ArgumentException("Cannot create order for a free table. Occupy the table first.");

            if (table.IsReserved)
                throw new ArgumentException("Cannot create order for a reserved table.");

            bool hasOpenOrder = orderRepository.HasOrderForTableWithStatus(
                tableId,
                OrderStatus.Open);

            if (hasOpenOrder)
                throw new ArgumentException("This table already has an active order.");

            Order order = new Order
            {
                TableId = tableId,
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Open,
                TotalAmount = 0
            };

            orderRepository.Add(order);

            return order;
        }

        public OrderItem AddItemToOrder(int orderId, int menuItemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0.");

            Order? order = orderRepository.GetById(orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.Status != OrderStatus.Open)
                throw new ArgumentException("Cannot add items to closed or cancelled order.");

            MenuItem? menuItem = menuItemRepository.GetById(menuItemId);

            if (menuItem == null)
                throw new ArgumentException("Menu item not found.");

            if (!menuItem.IsActive)
                throw new ArgumentException("Cannot add inactive menu item.");

            OrderItem? existingItem = orderItemRepository.GetByOrderAndMenuItem(
                orderId,
                menuItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                orderItemRepository.Update(existingItem);

                return existingItem;
            }

            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                MenuItemId = menuItemId,
                Quantity = quantity,
                UnitPrice = menuItem.Price,
                IsServed = false
            };

            orderItemRepository.Add(orderItem);

            return orderItem;
        }

        public void RemoveItemFromOrder(int orderItemId)
        {
            OrderItem? orderItem = orderItemRepository.GetById(orderItemId);

            if (orderItem == null)
                throw new ArgumentException("Order item not found.");

            if (orderItem.Order == null)
                throw new ArgumentException("Order not found.");

            if (orderItem.Order.Status != OrderStatus.Open)
                throw new ArgumentException("Cannot remove items from closed or cancelled order.");

            orderItemRepository.Delete(orderItem);
        }

        public Order ChangeOrderStatus(int orderId, OrderStatus status)
        {
            Order? order = orderRepository.GetById(orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.Status == OrderStatus.Closed)
                throw new ArgumentException("Closed order status cannot be changed.");

            order.Status = status;

            orderRepository.Update(order);

            return order;
        }

        public decimal CalculateTotal(int orderId)
        {
            Order? order = orderRepository.GetById(orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            List<OrderItem> orderItems = orderItemRepository.GetByOrderId(orderId);

            decimal total = orderItems
                .Sum(oi => oi.Quantity * oi.UnitPrice);

            return total;
        }

        public Order CloseOrder(int orderId)
        {
            Order? order = orderRepository.GetById(orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.Status != OrderStatus.Open)
                throw new ArgumentException("Only open orders can be closed.");

            decimal total = CalculateTotal(orderId);

            order.TotalAmount = total;
            order.Status = OrderStatus.Closed;

            Table? table = tableRepository.GetById(order.TableId);

            if (table != null)
            {
                table.IsOccupied = false;
                table.IsReserved = false;
                table.ReservedBy = null;

                tableRepository.Update(table);
            }

            orderRepository.Update(order);

            return order;
        }
    }
}