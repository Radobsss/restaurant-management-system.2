using System.Collections.Generic;
using restaurant_management_system._2.Domain.Entities;

namespace restaurant_management_system._2.Infrastructure
{
    public class TableStorage
    {
        public int NextId { get; set; } = 1;

        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}