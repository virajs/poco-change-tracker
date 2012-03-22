
namespace POCO.Monitoring.Tests
{
    [POCO.Monitoring.ObjectState.MonitorPocoState]
    public class OrderLine
    {
        public virtual Product Product { get; set; }

        public virtual int Quantity { get; set; }
    }
}
