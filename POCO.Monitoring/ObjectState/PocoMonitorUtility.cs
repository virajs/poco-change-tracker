using System;
using System.Collections.Generic;
using System.Threading;
using Castle.DynamicProxy;
using POCO.Monitoring.ObjectState.Implementation;

namespace POCO.Monitoring.ObjectState
{
    public static class PocoMonitorUtility
    {
        private static readonly object Locker = new object();

        private static readonly bool IsInitialized = false;

        private static ProxyGenerationOptions GenerationOptions { get; set; }

        private static ProxyGenerator ProxyGenerator { get; set; }

        private static readonly IDictionary<Type, Type> ProxyTypeDictionary;

        static PocoMonitorUtility()
        {
            lock (Locker)
            {
                if (IsInitialized)
                {
                    return;
                }

                GenerationOptions = new ProxyGenerationOptions(new ProxyGenerationHook());
                GenerationOptions.AddMixinInstance(new ObjectStateManager());

                ProxyGenerator = new ProxyGenerator(new DefaultProxyBuilder(new ModuleScope(false)));

                ProxyTypeDictionary = new Dictionary<Type, Type>();
                
                IsInitialized = true;
            }
        }

        public static void RegisterType<T>()
        {
            var actualType = typeof (T);
            var proxyType = ProxyGenerator.ProxyBuilder.CreateClassProxyTypeWithTarget(actualType,
                                                                                       new[] { typeof (IObjectStateManager) },
                                                                                       GenerationOptions);
            if(ProxyTypeDictionary.ContainsKey(actualType))
            {
                ProxyTypeDictionary.Remove(actualType);
            }

            ProxyTypeDictionary.Add(actualType, proxyType);
        }

        public static void PrepareContainer()
        {
            ProxyGenerator.ProxyBuilder.ModuleScope.SaveAssembly(false);
            var moduleBuilder = ProxyGenerator.ProxyBuilder.ModuleScope.ObtainDynamicModule(false);
            ProxyGenerator.ProxyBuilder.ModuleScope.LoadAssemblyIntoCache(moduleBuilder.Assembly);
        }

        public static T BeginMonitoring<T>(T @object)
            where T : class
        {
            if(@object == null)
            {
                return default(T);
            }

            var proxyType = GetProxyType<T>();

            if(proxyType != null)
            {
                var proxy = (IObjectStateManager)Activator.CreateInstance(proxyType, @object, new ObjectStateManager(), new [] { new PocoPropertyValueChangeInterceptor() });
                proxy.SetDocument(@object);

                return (T)proxy;
            }

            return @object;
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

        private static Type GetProxyType<T>()
            where T : class
        {
            var actualType = typeof(T);
            if(ProxyTypeDictionary.ContainsKey(actualType))
            {
                return ProxyTypeDictionary[actualType];
            }

            return ProxyGenerator.ProxyBuilder.CreateClassProxyTypeWithTarget(actualType,
                                                                              new[] {typeof (IObjectStateManager)},
                                                                              GenerationOptions);
        }
    }
}
