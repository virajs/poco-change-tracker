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
        [Test]
        public void Verify_customer_name_can_be_restored()
        {
            var customer = new Customer()
                               {
                                   Id = 8569,
                                   Name = "Mahela Jayawardene"
                               };

            MeasureTime(() =>
                            {
                                customer = PocoMonitorUtility.BeginMonitoring(customer);
                            });

            MeasureTime(() => Assert.AreEqual(8569, customer.Id));

            MeasureTime(() => Assert.AreEqual("Mahela Jayawardene", customer.Name));

            MeasureTime(() =>
                            {
                                customer.Name = "Jayawardene, Mahela";
                                Assert.AreEqual("Jayawardene, Mahela", customer.Name);
                            });

            MeasureTime(() => PocoMonitorUtility.Undo(customer));

            MeasureTime(() =>
                            {
                                Assert.AreEqual(8569, customer.Id);
                                Assert.AreEqual("Mahela Jayawardene", customer.Name);
                            });
        }

        private static void MeasureTime(Action @action)
        {
            var before = DateTime.Now;
            @action();
            var after = DateTime.Now;
            Console.WriteLine("Time taken : {0} ms", after.Subtract(before).TotalMilliseconds);
        }
    }
}
