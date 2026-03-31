using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using restaurant_management_system._2.Domain.Entities;

namespace RestaurantManagementSystem.Tests.EntityTests
{
    [TestClass]
    public class TableTests
    {
        [TestMethod]
        public void Table_ShouldInitializeOrdersList()
        {
            var table = new Table();

            Assert.IsNotNull(table.Orders);
        }
    }
}
