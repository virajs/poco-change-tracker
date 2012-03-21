using System;
using System.Linq;
using System.Reflection;

namespace POCO.Monitoring.ObjectState.Implementation
{
    [Serializable]
    public class ObjectStateManager : IObjectStateManager
    {
        private object _parent = null;

        private Type _parentType = null;

        private PropertyInfo[] _properties = null;

        private ObjectStateContainer _changeContainer = null;

        public object Target { get { return _parentType; } }

        public ObjectStateContainer ChangeContainer
        {
            get { return _changeContainer; }
        }

        object IObjectStateManager.GetDocument()
        {
            return _parent;
        }

        void IObjectStateManager.SetDocument(object document)
        {
            _parent = document;
            if (_parent != null)
            {
                _changeContainer = new ObjectStateContainer(this, _parent);
                _parentType = _parent.GetType();
                _properties = _parentType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            }
        }

        void IObjectStateManager.OnPropertyChanged(string propertyName, object value)
        {
            ChangeContainer.PushChanges(propertyName, value);
        }

        void IObjectStateManager.Undo()
        {
            if (ChangeContainer != null && ChangeContainer.CanUndo)
            {
                ChangeContainer.UndoChanges();
            }
        }

        void IObjectStateManager.Redo()
        {
            if (ChangeContainer != null && ChangeContainer.CanRedo)
            {
                ChangeContainer.RedoChanges();
            }
        }

        public PropertyInfo GetProperty(string name)
        {
            PropertyInfo property = null;
            if (_properties != null && _properties.Length > 0)
            {
                if (!_properties.Any(p => p.Name == name))
                {
                    throw new ApplicationException(string.Format("{0} property not found.", name));
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
    }
}