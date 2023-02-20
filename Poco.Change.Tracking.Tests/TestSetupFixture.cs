namespace Poco.Change.Tracking.Tests
{
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
