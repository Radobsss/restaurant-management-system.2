using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using restaurant_management_system._2.Domain.Enums;


namespace restaurant_management_system._2.Domain.Entities
{
    public class MenuItem
    {
        public Category Category { get; set; }
        public MenuItemType Type { get; set; }
    }
}
