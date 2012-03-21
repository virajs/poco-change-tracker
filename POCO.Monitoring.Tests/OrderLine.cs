
namespace POCO.Monitoring.Tests
{
    public class OrderLine
    {
        public virtual int OrderId { get; set; }

        public virtual Product Product { get; set; }

        public virtual int Quantity { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
