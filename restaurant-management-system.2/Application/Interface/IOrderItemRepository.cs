using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Application.Interface
{
    public interface IOrderItemRepository
    {
        List<OrderItem> GetByOrderId(int orderId);

        OrderItem? GetById(int id);

        OrderItem? GetByOrderAndMenuItem(int orderId, int menuItemId);

        void Add(OrderItem orderItem);

        void Update(OrderItem orderItem);

        void Delete(OrderItem orderItem);
    }
}