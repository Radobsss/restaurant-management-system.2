using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace RestaurantManagementSystem.Tests.EntityTests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
            public void Order_ShouldInitializeItemsList()
            {
                var order = new Order();

                Assert.IsNotNull(order.Items);
            }
        [TestMethod]
        public void Order_ShouldSetAndGetStatus()
        {
            var order = new Order
            {
                Status = OrderStatus.Open
            };

            Assert.AreEqual(OrderStatus.Open, order.Status);
        }

        }
    }
}


