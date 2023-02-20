using Castle.DynamicProxy;

namespace Poco.Change.Tracking
{
    public class PocoPropertyValueChangeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.IsPublic && (invocation.Method.Name.StartsWith("get_", StringComparison.Ordinal) || invocation.Method.Name.StartsWith("set_", StringComparison.Ordinal)))
            {
                if (invocation.Proxy is IPocoStateManagerContainer undoRedo)
                {
                    string propertyName = invocation.Method.Name.Replace("set_", string.Empty).Replace("get_",
                                                                                                       string.Empty);
                    if (invocation.Method.ReturnType == typeof (void))
                    {
                        if (invocation.Arguments is { Length: > 0 })
                        {
                            //undoRedo.OnPropertyChanged(propertyName, undoRedo.GetPropertyValue(propertyName));
                            if (!undoRedo.HasParent())
                            {
                                undoRedo.SetTarget(invocation.Proxy);
                            }
                            var propertyValue = invocation.Arguments.First();
                            undoRedo.OnPropertyChanged(propertyName, propertyValue);
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