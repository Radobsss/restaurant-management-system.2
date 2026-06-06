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
        //[TestMethod]
        //public void Table_ShouldInitializeOrdersList()
        //{
        //    var table = new Table();

        //    Assert.IsNotNull(table.Orders);
        //}

        //[TestMethod]
        //public void Table_ShouldInitializeReservationsList()
        //{
        //    var table = new Table();

        //    Assert.IsNotNull(table.Reservations);
        //}

        [TestMethod]
        public void Table_ShouldSetAndGetPropertiesCorrectly()
        {
            var table = new Table
            {
                Id = 1,
                Number = 5,
                Capacity = 4,
                IsOccupied = true
            };

            Assert.AreEqual(1, table.Id);
            Assert.AreEqual(5, table.Number);
            Assert.AreEqual(4, table.Capacity);
            Assert.IsTrue(table.IsOccupied);
        }
    }
}
