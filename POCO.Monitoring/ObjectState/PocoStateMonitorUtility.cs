using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using POCO.Monitoring.ObjectState.Implementation;

namespace POCO.Monitoring.ObjectState
{
    public class PocoStateMonitorUtility
    {
        private static PocoStateMonitorUtility _instance;

        private static readonly object Locker = new object();

        private static bool _isInitialized;

        private ProxyGenerationOptions GenerationOptions { get; set; }

        private ProxyGenerator ProxyGenerator { get; set; }

        private readonly IDictionary<Type, Type> _proxyTypeDictionary;

        PocoStateMonitorUtility()
        {
            lock (Locker)
            {
                if (_isInitialized)
                {
                    return;
                }

                GenerationOptions = new ProxyGenerationOptions(new ProxyGenerationHook());
                GenerationOptions.AddMixinInstance(new DefaultPocoStateManagerContainer());

                ProxyGenerator = new ProxyGenerator(new DefaultProxyBuilder(new ModuleScope(true)));

                _proxyTypeDictionary = new Dictionary<Type, Type>();
            }
        }

        public static PocoStateMonitorUtility Current
        {
            get
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        if (!_isInitialized)
                        {
                            _instance = new PocoStateMonitorUtility();
                            PocoStateMonitorUtility._isInitialized = true;
                        }
                    }
                    return _instance;
                }
            }
        }

        public void RegisterType<T>()
        {
            var actualType = typeof (T);
            var proxyType = ProxyGenerator.ProxyBuilder.CreateClassProxyTypeWithTarget(actualType,
                                                                                       new[] { typeof (IPocoStateManagerContainer) },
                                                                                       GenerationOptions);
            if(_proxyTypeDictionary.ContainsKey(actualType))
            {
                _proxyTypeDictionary.Remove(actualType);
            }

            _proxyTypeDictionary.Add(actualType, proxyType);
        }

        public void PrepareContainer()
        {
            ProxyGenerator.ProxyBuilder.ModuleScope.SaveAssembly(false);
            var moduleBuilder = ProxyGenerator.ProxyBuilder.ModuleScope.ObtainDynamicModule(false);
            ProxyGenerator.ProxyBuilder.ModuleScope.LoadAssemblyIntoCache(moduleBuilder.Assembly);
        }

        public T BeginMonitoring<T>(T @object)
            where T : class
        {
            if(@object == null)
            {
                return default(T);
            }

            var proxyType = GetProxyType<T>();

            if(proxyType != null)
            {
                var container = new DefaultPocoStateManagerContainer();
                var proxy = (IPocoStateManagerContainer) Activator.CreateInstance(proxyType,
                                                                                  @object,
                                                                                  container,
                                                                                  container,
                                                                                  new[] { new PocoPropertyValueChangeInterceptor() });
                proxy.SetTarget(@object);

                return (T)proxy;
            }

            return @object;
        }

        public void Undo(object @object)
        {
            var tracker = @object as IPocoStateManagerContainer;
            if (tracker != null)
            {
                tracker.Undo();
            }
        }

        public void Redo(object @object)
        {
            var tracker = @object as IPocoStateManagerContainer;
            if (tracker != null)
            {
                tracker.Redo();
            }
        }

        public bool IsDirty(object @object)
        {
            return false;
        }

        private Type GetProxyType<T>()
            where T : class
        {
            var actualType = typeof(T);
            if(_proxyTypeDictionary.ContainsKey(actualType))
            {
                return _proxyTypeDictionary[actualType];
            }

            return ProxyGenerator.ProxyBuilder.CreateClassProxyTypeWithTarget(actualType,
                                                                              new[] {typeof (IPocoStateManagerContainer)},
                                                                              GenerationOptions);
        }
    }
}
