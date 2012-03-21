using NUnit.Framework;
using POCO.Monitoring.ObjectState;

namespace POCO.Monitoring.Tests
{
    [TestFixture]
    public class ProxyVerificationTest
    {
        [Test]
        public void Verify_a_proxy_is_returned_when_state_monitoring_is_started()
        {
            Verify_instance_is_a_proxy(new Customer());
            Verify_instance_is_a_proxy(new Product());
            Verify_instance_is_a_proxy(new Order());
            Verify_instance_is_a_proxy(new OrderLine());
        }

        private void Verify_instance_is_a_proxy(object @object)
        {
            Assert.IsTrue(Castle.DynamicProxy.ProxyUtil.IsProxy(PocoMonitorUtility.BeginMonitoring(@object)));
        }
    }
}
