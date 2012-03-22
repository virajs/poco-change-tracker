using System.Reflection;

namespace POCO.Monitoring.ObjectState.Implementation
{
    public interface IPocoStateManagerContainer : IPocoStateManager
    {
        ObjectStateContainer Container { get; }

        object GetTarget();

        void SetTarget(object document);

        PropertyInfo GetProperty(string name);
        
        object GetPropertyValue(string name);

        void OnPropertyChanged(string propertyName, object value);

        void InsertChildMonitor(IPocoStateManagerContainer child);
    }

    public interface IPocoStateManager
    {
        void Undo();

        void Redo();
    }
}