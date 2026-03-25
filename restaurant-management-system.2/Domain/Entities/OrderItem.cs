using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restaurant_management_system._2.Domain.Entities
{
    public class OrderItem
    {
        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
