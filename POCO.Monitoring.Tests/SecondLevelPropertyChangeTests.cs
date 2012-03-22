using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using POCO.Monitoring.ObjectState;

namespace POCO.Monitoring.Tests
{
    [TestFixture]
    public class SecondLevelPropertyChangeTests : BaseTest
    {
        [Test]
        public void Verify_a_second_level_undo_works()
        {
            var customer = new Customer()
                               {
                                   Id = 8569,
                                   Name = "Mahela Jayawardene"
                               };

            var jam = new Product()
                              {
                                  Name = "Jam",
                                  Price = 121.50
                              };

            var cheese = new Product()
                             {
                                 Name = "Cheese",
                                 Price = 121.50
                             };

            var order = new Order()
                            {
                                Customer = customer,
                                OrderDate = DateTime.UtcNow,
                                IsPaid = true,
                                Lines = new List<OrderLine>()
                                            {
                                                new OrderLine()
                                                    {
                                                        Product = jam,
                                                        Quantity = 3
                                                    },
                                                new OrderLine()
                                                    {
                                                        Product = cheese,
                                                        Quantity = 2
                                                    }
                                            }
                            };

            Assert.AreEqual("Mahela Jayawardene", order.Customer.Name);

            order = PocoStateMonitorUtility.Current.BeginMonitoring(order);

            order.Customer.Name = "Jayawardene, Mahela";
            Assert.AreEqual("Jayawardene, Mahela", order.Customer.Name);
            
            PocoStateMonitorUtility.Current.Undo(order);
            Assert.AreEqual("Mahela Jayawardene", order.Customer.Name);
        }
    }
}
