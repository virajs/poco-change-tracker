
namespace Poco.Change.Tracking.Tests
{
    [MonitorPocoState]
    public class Product
    {
        public virtual string Name { get; set; }

        public virtual double Price { get; set; }
    }
}
