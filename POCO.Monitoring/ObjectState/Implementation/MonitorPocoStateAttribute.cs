using System;

namespace POCO.Monitoring.ObjectState.Implementation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MonitorPocoStateAttribute : Attribute
    {
    }
}
