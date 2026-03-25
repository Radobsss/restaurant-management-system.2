using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restaurant_management_system._2.Domain.Entities
{
    public class Table
    {
        public List<Order> Orders { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
