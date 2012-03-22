using System;
using NUnit.Framework;
using POCO.Monitoring.ObjectState;

namespace POCO.Monitoring.Tests
{
    [TestFixture]
    public class FirstLevelPropertyChangeTests : BaseTest
    {
        [Test]
        public void Verify_customer_name_can_be_restored()
        {
            var customer = new Customer()
                               {
                                   Id = 8569,
                                   Name = "Mahela Jayawardene"
                               };

            MeasureTime("[customer = PocoStateMonitorUtility.Current.BeginMonitoring(customer)]", () =>
                            {
                                customer = PocoStateMonitorUtility.Current.BeginMonitoring(customer);
                            });

            MeasureTime("[var id = customer.Id]", () => { var id = customer.Id; });

            MeasureTime("[Assert.AreEqual(8569, customer.Id)]", () => Assert.AreEqual(8569, customer.Id));

            MeasureTime("[Assert.AreEqual('Mahela Jayawardene', customer.Name)]", () => Assert.AreEqual("Mahela Jayawardene", customer.Name));

            MeasureTime("[customer.Name = 'Jayawardene, Mahela']", () => { customer.Name = "Jayawardene, Mahela";});

            MeasureTime("[Assert.AreEqual('Jayawardene, Mahela', customer.Name)]", () => Assert.AreEqual("Jayawardene, Mahela", customer.Name));

            MeasureTime("[PocoStateMonitorUtility.Current.Undo(customer)]", () => PocoStateMonitorUtility.Current.Undo(customer));

            MeasureTime("[customer.Id = 1253]", () => customer.Id = 1253);

            MeasureTime("[Assert.AreEqual(1253, customer.Id)]", () => Assert.AreEqual(1253, customer.Id));

            MeasureTime("[PocoStateMonitorUtility.Current.Undo(customer)]", () => PocoStateMonitorUtility.Current.Undo(customer));

            MeasureTime("[Assert.AreEqual(8569, customer.Id)]", () => Assert.AreEqual(8569, customer.Id));

            MeasureTime("[Assert.AreEqual('Mahela Jayawardene', customer.Name)]", () => Assert.AreEqual("Mahela Jayawardene", customer.Name));
        }

        
    }
}
