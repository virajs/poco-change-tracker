namespace Poco.Change.Tracking.Tests
{
    [MonitorPocoState]
    public class Order
    {
        public virtual Customer Customer { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual bool? IsPaid { get; set; }

        public virtual List<OrderLine> Lines { get; set; }
    }
}
