using System.Reflection;

namespace Poco.Change.Tracking
{
    [Serializable]
    public sealed class DefaultPocoStateManagerContainer : IPocoStateManagerContainer
    {
        private object _parent;

        private Type _parentType;

        private PropertyInfo[] _properties;

        private ObjectStateContainer _container;

        public object Target => _parentType;

        public ObjectStateContainer Container => _container;

        public object GetTarget()
        {
            return _parent;
        }

        public bool HasParent() => this._parent != null;

        public void SetTarget(object document)
        {
            _parent = document;
            if (_parent != null)
            {
                _container = new ObjectStateContainer(this, _parent);
                _parentType = _parent.GetType();
                _properties = _parentType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            }
        }

        public void OnPropertyChanged(string propertyName, object value)
        {
            _container.PushChanges(propertyName, value);
        }

        public void Undo()
        {
            if (_container != null && _container.CanUndo)
            {
                _container.UndoChanges();
                
            }
        }

        public void Redo()
        {
            if (_container != null && _container.CanRedo)
            {
                _container.RedoChanges();
            }
        }

        public PropertyInfo GetProperty(string name)
        {
            PropertyInfo property = null;
            if (_properties != null && _properties.Length > 0)
            {
                if (_properties.All(p => p.Name != name))
                {
                    throw new ApplicationException($"{name} property not found.");
                }

                property = _properties.SingleOrDefault(p => p.Name == name);
            }
            return property;
        }

        public object GetPropertyValue(string name)
        {
            PropertyInfo property = GetProperty(name);
            return property.GetValue(_parent, null);
        }

        public void InsertChildMonitor(IPocoStateManagerContainer child)
        {
            _container.InsertChildTracker(child.Container);
        }
    }
}