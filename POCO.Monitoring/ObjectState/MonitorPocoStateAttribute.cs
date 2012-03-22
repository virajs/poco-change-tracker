using System;

namespace POCO.Monitoring.ObjectState
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MonitorPocoStateAttribute : Attribute
    {
    }
}
