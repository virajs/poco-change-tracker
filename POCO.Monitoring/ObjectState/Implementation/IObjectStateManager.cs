using System.Reflection;

namespace POCO.Monitoring.ObjectState.Implementation
{
    public interface IObjectStateManager
    {
        ObjectStateContainer ChangeContainer { get; }

        object GetDocument();

        void SetDocument(object document);

        void Undo();

        void Redo();

        PropertyInfo GetProperty(string name);
        
        object GetPropertyValue(string name);

        void OnPropertyChanged(string propertyName, object value);
    }
}