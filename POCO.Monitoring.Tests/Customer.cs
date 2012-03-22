﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCO.Monitoring.Tests
{
    [POCO.Monitoring.ObjectState.MonitorPocoState]
    public class Customer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }
}
