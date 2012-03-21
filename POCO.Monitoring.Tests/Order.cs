using System;
using System.Collections.Generic;

namespace POCO.Monitoring.Tests
{
    public class Order
    {
        public virtual DateTime OrderDate { get; set; }

        public virtual bool? IsPaid { get; set; }

        public virtual List<OrderLine> Lines { get; set; }
    }
}
