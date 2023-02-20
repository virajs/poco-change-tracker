using System.Reflection;

namespace Poco.Change.Tracking
{
    public interface IPocoStateManagerContainer : IPocoStateManager
    {
        ObjectStateContainer Container { get; }

        object GetTarget();

        bool HasParent();

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