namespace Poco.Change.Tracking.Tests
{
    [MonitorPocoState]
    public class Customer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        
        public   virtual IList<Order> Orders { get; set; }

        public Customer()
        {
            Orders = new List<Order>();
        }
    }
    
}
