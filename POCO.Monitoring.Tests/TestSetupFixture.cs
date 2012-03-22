using NUnit.Framework;
using POCO.Monitoring.ObjectState;

namespace POCO.Monitoring.Tests
{
    [SetUpFixture]
    public class TestSetupFixture
    {
        [SetUp]
        public void Setup()
        {
            PocoStateMonitorUtility.Current.RegisterType<Customer>();
            PocoStateMonitorUtility.Current.RegisterType<Product>();
            PocoStateMonitorUtility.Current.RegisterType<Order>();
            PocoStateMonitorUtility.Current.RegisterType<OrderLine>();
            PocoStateMonitorUtility.Current.PrepareContainer();
        }
    }
}
