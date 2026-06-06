using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;

namespace restaurant_management_system._2.Application.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAll();

        Order? GetById(int id);

        List<Order> GetByTableId(int tableId);

        bool HasOrderForTableWithStatus(int tableId, OrderStatus status);

        void Add(Order order);

        void Update(Order order);
    }
}