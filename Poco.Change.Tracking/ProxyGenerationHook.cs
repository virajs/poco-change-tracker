using System.Reflection;
using Castle.DynamicProxy;

namespace Poco.Change.Tracking
{
    [Serializable]
    public class ProxyGenerationHook : IProxyGenerationHook
    {
        protected static readonly ICollection<Type> SkippedTypes = new Type[] { typeof(object), typeof(MarshalByRefObject), typeof(ContextBoundObject) };

        protected bool IsFinalizer(MethodInfo methodInfo)
        {
            return ((methodInfo.Name == "Finalize") && (methodInfo.GetBaseDefinition().DeclaringType == typeof(object)));
        }

        public void MethodsInspected()
        {
            
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            bool shouldInterceptMethod = (!SkippedTypes.Contains(methodInfo.DeclaringType) &&
                                          !this.IsFinalizer(methodInfo) &&
                                          methodInfo.IsPublic &&
                                          (methodInfo.Name.StartsWith("get_", StringComparison.Ordinal) ||
                                           methodInfo.Name.StartsWith("set_", StringComparison.Ordinal)));
            return shouldInterceptMethod;
        }
    }
}