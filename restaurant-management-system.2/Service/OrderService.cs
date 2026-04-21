using restaurant_management_system._2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restaurant_management_system._2.Service
{
    public class OrderService
    {
        private readonly List<Order> orders = new List<Order>();
        private readonly TableService tableService;

        public OrderService(TableService tableService)
        {
            this.tableService = tableService;
        }

        public Order CreateOrder(int tableId)
        {
            Table table = tableService.GetTableById(tableId);

            if (table.IsOccupied)
                throw new InvalidOperationException("Table is already occupied.");

            Order order = new Order
            {
                Id = orders.Count + 1,
                TableId = tableId,
                CreatedAt = DateTime.Now
            };

            orders.Add(order);
            table.IsOccupied = true;

            return order;
        }
    }
}
