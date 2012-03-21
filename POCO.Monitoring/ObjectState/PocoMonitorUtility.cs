using System;
using Castle.DynamicProxy;
using POCO.Monitoring.ObjectState.Implementation;

namespace POCO.Monitoring.ObjectState
{
    public static class PocoMonitorUtility
    {
        private static readonly object Locker = new object();

        private static ProxyGenerator ProxyGenerator { get; set; }

        static PocoMonitorUtility()
        {
            lock (Locker)
            {
                if (ProxyGenerator == null)
                {
                    ProxyGenerator = new ProxyGenerator(new DefaultProxyBuilder());
                }
            }
        }

        public static T BeginMonitoring<T>(T @object)
            where T : class
        {
            if(@object == null)
            {
                return default(T);
            }

            Type nominalType = @object.GetType();

            var proxyGenerationOptions = new ProxyGenerationOptions(new ProxyGenerationHook());
            proxyGenerationOptions.AddMixinInstance(new ObjectStateManager());

            return (T) ProxyGenerator.CreateClassProxyWithTarget(nominalType,
                                                                 new[] {typeof (IObjectStateManager)},
                                                                 @object,
                                                                 proxyGenerationOptions,
                                                                 new PocoPropertyValueChangeInterceptor());
        }

        public static void Undo(object @object)
        {
            var tracker = @object as IObjectStateManager;
            if (tracker != null)
            {
                tracker.Undo();
            }
        }

        public static void Redo(object @object)
        {
            var tracker = @object as IObjectStateManager;
            if (tracker != null)
            {
                tracker.Redo();
            }
        }

        public static bool IsDirty(object @object)
        {
            return false;
        }
    }
}
