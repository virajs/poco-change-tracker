using System;

namespace POCO.Monitoring.Tests
{
    public abstract class BaseTest
    {
        protected void MeasureTime(string operation, Action @action)
        {
            var before = DateTime.Now;
            @action();
            var after = DateTime.Now;
            Console.WriteLine("{0} has taken {1} ms to complete.", 
                string.IsNullOrEmpty(operation) ? "[Operation]" : operation, 
                after.Subtract(before).TotalMilliseconds);
        }
    }
}
