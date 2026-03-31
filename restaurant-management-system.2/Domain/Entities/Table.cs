using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restaurant_management_system._2.Domain.Entities
{
    public class Table
    {
        public List<Order> Orders { get; set; }  = new List<Order>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public int Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }
       
    }
}
