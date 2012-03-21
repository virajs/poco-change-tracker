using System;
using System.Linq;
using System.Collections.Generic;

namespace POCO.Monitoring.ObjectState.Implementation
{
    [Serializable]
    public sealed class ObjectStateContainer
    {
        private readonly Stack<Array> _redoStack;

        private readonly Stack<Array> _undoStack;

        private readonly Stack<IObjectStateManager> _children;

        private readonly object _parent;

        private readonly IObjectStateManager _stateManager;

        public ObjectStateContainer(IObjectStateManager stateManager, object parent)
        {
            _stateManager = stateManager;
            _redoStack = new Stack<Array>();
            _undoStack = new Stack<Array>();
            _children = new Stack<IObjectStateManager>();
            _parent = parent;
        }

        public void InsertChildTracker(IObjectStateManager childStateManager)
        {
            if(!_children.Contains(childStateManager))
            {
                _children.Push(childStateManager);
            }
        }

        public void PushChanges(string propName, object propVal)
        {
            _undoStack.Push(new [] { propName, propVal });
        }

        public void UndoChanges()
        {
            PerformAction(undo: true);
        }

        public void RedoChanges()
        {
            PerformAction(undo: false);
        }

        public bool CanUndo
        {
            get
            {
                var canUndo = (_undoStack.Count > 0 ? true : false);
                if(_children.Count > 0)
                {
                    canUndo = (canUndo || _children.Any(p => p.ChangeContainer.CanUndo));
                }
                return canUndo;
            }
        }

        public bool CanRedo
        {
            get
            {
                var canRedo = (_redoStack.Count > 0 ? true : false);
                if (_children.Count > 0)
                {
                    canRedo = (canRedo || _children.Any(p => p.ChangeContainer.CanRedo));
                }
                return canRedo;
            }
        }

        private void PerformAction(bool undo)
        {
            var mainStack = _undoStack;
            var secondaryStack = _redoStack;

            if(!undo)
            {
                mainStack = secondaryStack;
                secondaryStack = _undoStack;
            }

            if (mainStack.Count > 0)
            {
                do
                {
                    var lastModification = mainStack.Pop();
                    WriteData(secondaryStack, lastModification.GetValue(0).ToString(), lastModification.GetValue(1),
                              undo);
                } while (mainStack.Count > 0);
            }

            if (_children.Count > 0)
            {
                foreach (var manager in _children)
                {
                    if (undo)
                    {
                        manager.Undo();
                    }
                    else
                    {
                        manager.Redo();
                    }
                }
            }
        }

        private void WriteData(Stack<Array> stack, string propertyName, object previousValue, bool undo)
        {
            var property = _stateManager.GetProperty(propertyName);

            var oldVal = property.GetValue(_parent, null);
            stack.Push(new[] { propertyName, oldVal });

            if (property.CanWrite)
            {
                property.SetValue(_parent, previousValue, null);
            }
            else
            {
                if (!property.PropertyType.IsPrimitive)
                {
                    RunAction(undo, property.GetValue(_parent, null));
                }
            }
        }

        private static void RunAction(bool undo, object value)
        {
            var objectStateManager = value as IObjectStateManager;
            if (objectStateManager != null)
            {
                if (undo)
                {
                    objectStateManager.Undo();
                }
                else
                {
                    objectStateManager.Redo();
                }
            }
        }
    }
}