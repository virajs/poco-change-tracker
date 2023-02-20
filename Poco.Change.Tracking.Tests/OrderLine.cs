
namespace Poco.Change.Tracking.Tests
{
    [MonitorPocoState]
    public class OrderLine
    {
        public virtual Product Product { get; set; }

        public virtual int Quantity { get; set; }
    }
}
