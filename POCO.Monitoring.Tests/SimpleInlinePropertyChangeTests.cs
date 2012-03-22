using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using POCO.Monitoring.ObjectState;

namespace POCO.Monitoring.Tests
{
    [TestFixture]
    public class SimpleInlinePropertyChangeTests
    {
        [SetUp]
        public void Setup()
        {
            MeasureTime("Init", () =>
                                    {
                                        PocoMonitorUtility.RegisterType<Customer>();
                                        PocoMonitorUtility.RegisterType<Product>();
                                        PocoMonitorUtility.RegisterType<Order>();
                                        PocoMonitorUtility.RegisterType<OrderLine>();

                                        PocoMonitorUtility.PrepareContainer();
                                    });
        }

        [Test]
        public void Verify_customer_name_can_be_restored()
        {
            var customer = new Customer()
                               {
                                   Id = 8569,
                                   Name = "Mahela Jayawardene"
                               };

            MeasureTime("Operation 1", () =>
                            {
                                customer = PocoMonitorUtility.BeginMonitoring(customer);
                            });

            MeasureTime("Operation 2", () => Assert.AreEqual(8569, customer.Id));

            MeasureTime("Operation 3", () => Assert.AreEqual("Mahela Jayawardene", customer.Name));

            MeasureTime("Operation 4", () =>
                            {
                                customer.Name = "Jayawardene, Mahela";
                                Assert.AreEqual("Jayawardene, Mahela", customer.Name);
                            });

            MeasureTime("Operation 5", () => PocoMonitorUtility.Undo(customer));

            MeasureTime("Operation 6", () =>
                            {
                                Assert.AreEqual(8569, customer.Id);
                                Assert.AreEqual("Mahela Jayawardene", customer.Name);
                            });
        }

        private static void MeasureTime(string operation, Action @action)
        {
            var before = DateTime.Now;
            @action();
            var after = DateTime.Now;
            Console.WriteLine("{0} has taken {1} ms to complete.", operation, after.Subtract(before).TotalMilliseconds);
        }
    }
}
