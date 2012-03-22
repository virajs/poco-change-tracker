using System;
using Castle.Core.Internal;
using Castle.DynamicProxy;

namespace POCO.Monitoring.ObjectState.Implementation
{
    public class PocoPropertyValueChangeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.IsPublic && (invocation.Method.Name.StartsWith("get_", StringComparison.Ordinal) || invocation.Method.Name.StartsWith("set_", StringComparison.Ordinal)))
            {
                var undoRedo = invocation.Proxy as IPocoStateManagerContainer;
                if (undoRedo != null)
                {
                    string propertyName = invocation.Method.Name.Replace("set_", string.Empty).Replace("get_",
                                                                                                       string.Empty);
                    if (invocation.Method.ReturnType == typeof (void))
                    {
                        if (invocation.Arguments != null && invocation.Arguments.Length > 0)
                        {
                            undoRedo.OnPropertyChanged(propertyName, undoRedo.GetPropertyValue(propertyName));
                        }
                    }
                    else
                    {
                        if (!invocation.Method.ReturnType.IsPrimitive)
                        {
                            var attribs = invocation.Method.ReturnType.GetCustomAttributes(typeof (MonitorPocoStateAttribute), true);
                            if (attribs.Length > 0)
                            {
                                invocation.Proceed();
                                undoRedo.InsertChildMonitor(invocation.ReturnValue as IPocoStateManagerContainer);
                                return;
                            }
                        }
                    }
                }
            }
            invocation.Proceed();
        }
    }
}