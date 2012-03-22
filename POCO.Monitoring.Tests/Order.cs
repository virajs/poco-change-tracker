using System;
using System.Collections.Generic;

namespace POCO.Monitoring.Tests
{
    [POCO.Monitoring.ObjectState.MonitorPocoState]
    public class Order
    {
        public virtual Customer Customer { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual bool? IsPaid { get; set; }

        public virtual List<OrderLine> Lines { get; set; }
    }
}
