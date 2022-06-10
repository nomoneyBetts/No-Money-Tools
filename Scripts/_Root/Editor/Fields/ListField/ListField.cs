using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// Base class for inputing lists.
    /// </summary>
    /// <typeparam name="T">The type of List.</typeparam>
    public abstract class ListField<T> : NoMoneyField<List<T>>
    {
        #region Properties
        private string _title;
        new public string Title
        {
            get => _title;
            set
            {
                if (value == null) return;

                if (this[0] is Foldout foldout) foldout.text = value;
                else if (this[0] is Label label) label.text = value;
                else Insert(0, new Label(value));

                _title = value;
            }
        }

        private bool _showIndices;
        public bool ShowIndices
        {
            get => _showIndices;
            set
            {
                _showIndices = value;
                UpdateIndices();
            }
        }

        private bool _isAdditive = true;
        public bool IsAdditive
        {
            get => _isAdditive;
            set
            {
                _isAdditive = value;
                VisualElement buttons = this.Q<VisualElement>(Buttons);
                if (value)
                {
                    buttons.style.height = new StyleLength(StyleKeyword.Auto);
                    buttons.style.visibility = Visibility.Visible;
                }
                else
                {
                    buttons.style.height = new StyleLength(0f);
                    buttons.style.visibility = Visibility.Hidden;
                }
            }
        }

        private bool _isCollapsible = true;
        public bool IsCollapsible
        {
            get => _isCollapsible;
            set
            {
                VisualElement elements = this.Q<VisualElement>(Elements);
                VisualElement buttons = this.Q<VisualElement>(Buttons);
                elements.RemoveFromHierarchy();
                buttons.RemoveFromHierarchy();
                Clear();
                if (value)
                {
                    Foldout foldout = new Foldout { text = Title };
                    foldout.Add(elements);
                    foldout.Add(buttons);
                    Add(foldout);
                }
                else
                {
                    Add(new Label(Title));
                    Add(elements);
                    Add(buttons);                    
                }

                _isCollapsible = value;
            }
        }

        private bool _isArrangeable = true;
        public bool IsArrangeable
        {
            get => _isArrangeable;
            set => _isArrangeable = value;
        }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                ((Foldout)ElementAt(0)).value = value;
            }
        }
        #endregion

        private VisualElement _selectedElement, _mouseOverElement, _dragElement;
        private bool _doubleRemove;

        private const string Elements = "elements";
        private const string ElementWrapper = "element-wrapper";
        private const string Buttons = "buttons-wrapper";
        private const string ElementHandle = "element-handle";
        private const string ElementField = "element-field";

        private readonly System.InvalidCastException MissingINotify =
            new System.InvalidCastException($"Field must implement INotifyValueChanged<{typeof(T).Name}>");

        public ListField()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("ListField");
            visualTree.CloneTree(this);

            VisualElement elements = this.Q<VisualElement>(Elements);
            // Set drag events
            elements.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                if (IsArrangeable && _dragElement != null && _dragElement != _mouseOverElement)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                }
            });
            elements.RegisterCallback<DragPerformEvent>(evt =>
            {
                evt.StopPropagation();
                if (!IsArrangeable || _dragElement == null) return;
                if (_dragElement != _mouseOverElement)
                {
                    List<T> previousValue = GetValue();
                    int placeIndex = elements.IndexOf(_mouseOverElement);
                    elements.Remove(_dragElement);
                    elements.Insert(placeIndex, _dragElement);
                    UpdateIndices();
                    List<T> newValue = GetValue();

                    using ChangeEvent<List<T>> change = ChangeEvent<List<T>>.GetPooled(previousValue, newValue);
                    change.target = this;
                    SendEvent(change);
                }
                _dragElement.Blur();
                _dragElement = null;
            });

            // Set the buttons
            VisualElement buttons = this.Q<VisualElement>(Buttons);
            ((Button)buttons.ElementAt(0)).clicked += () =>
            {
                List<T> previousValue = GetValue();
                elements.Add(CreateElement());
                List<T> newValue = GetValue();
                UpdateIndices();
                using ChangeEvent<List<T>> change = ChangeEvent<List<T>>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            };
            ((Button)buttons.ElementAt(1)).RegisterCallback<FocusInEvent>(evt =>
            {
                _doubleRemove = true;
                RemoveElement();
            });
            ((Button)buttons.ElementAt(1)).clicked += () =>
            {
                if (!_doubleRemove || _selectedElement == null) RemoveElement();
                _doubleRemove = false;
            };

            Title = $"{typeof(T).Name}s";

            void RemoveElement()
            {
                List<T> previousValue = GetValue();
                if (previousValue.Count == 0) return;
                if (_selectedElement != null)
                {
                    elements.Remove(_selectedElement);
                    _selectedElement = null;
                }
                else
                {
                    elements.RemoveAt(elements.childCount - 1);
                }
                List<T> newValue = GetValue();
                UpdateIndices();
                using ChangeEvent<List<T>> change = ChangeEvent<List<T>>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            }
        }

        public ListField(string title) : this()
        {
            Title = title;
        }

        #region Set and Get
        public override void SetValueWithoutNotify(List<T> newValue)
        {
            VisualElement elements = this.Q<VisualElement>(Elements);
            elements.Clear();
            if (newValue == null) return;
            foreach(T value in newValue)
            {
                VisualElement element = CreateElement();
                INotifyValueChanged<T> notifier = (INotifyValueChanged<T>)element.ElementAt(1);
                notifier.SetValueWithoutNotify(value);
                elements.Add(element);
            }
            UpdateIndices();
        }

        /// <summary>
        /// Sets the element at the index without notifying the event system.
        /// </summary>
        public void SetElementWithoutNotify(int index, T value)
        {
            VisualElement elements = this.Q<VisualElement>(Elements);
            VisualElement element = elements.ElementAt(index);
            INotifyValueChanged<T> notifier = (INotifyValueChanged<T>)element.ElementAt(1);
            notifier.SetValueWithoutNotify(value);
        }

        /// <summary>
        /// Sets the element at the index.
        /// </summary>
        public void SetElement(int index, T value)
        {
            VisualElement elements = this.Q<VisualElement>(Elements);
            VisualElement element = elements.ElementAt(index);
            INotifyValueChanged<T> notifier = (INotifyValueChanged<T>)element.ElementAt(1);
            notifier.value = value;
        }

        /// <typeparam name="S">The type of field the list holds.</typeparam>
        /// <returns>A list of the fields contained in this list field.</returns>
        public List<S> GetFields<S>() where S: VisualElement
        {
            List<S> fields = new List<S>();
            VisualElement elements = this.Q<VisualElement>(Elements);
            foreach(VisualElement wrapper in elements.Children())
            {
                S element;
                if ((element = wrapper.Q<S>(className: ElementField)) != null) fields.Add(element);
            }
            return fields;
        }

        protected override List<T> GetValue()
        {
            VisualElement elements = this.Q<VisualElement>(Elements);
            List<T> list = new List<T>();
            foreach(VisualElement element in elements.Children())
            {
                list.Add(((INotifyValueChanged<T>)element.ElementAt(1)).value);
            }
            return list;
        }

        protected override void SetValue(List<T> value)
        {
            VisualElement elements = this.Q<VisualElement>(Elements);
            elements.Clear();
            if (value == null) return;
            foreach(T v in value)
            {
                VisualElement element = CreateElement();
                INotifyValueChanged<T> notifier = (INotifyValueChanged<T>)element.ElementAt(1);
                notifier.value = v;
                elements.Add(element);
            }
            UpdateIndices();
        }
        #endregion

        #region Private Helpers
        /// <returns>An element to be displayed in the list.</returns>
        private VisualElement CreateElement()
        {
            VisualElement elementWrapper = new VisualElement();
            elementWrapper.AddToClassList(ElementWrapper);
            elementWrapper.focusable = true;

            Label handle = new Label();
            handle.AddToClassList(ElementHandle);

            VisualElement field = CreateField();
            if (!(field is INotifyValueChanged<T>)) throw MissingINotify;
            INotifyValueChanged<T> notifier = (INotifyValueChanged<T>)field;
            notifier.RegisterValueChangedCallback(evt =>
            {
                evt.StopPropagation();
                // Collect the values
                VisualElement elements = this.Q<VisualElement>(Elements);
                List<T> previousValue = new List<T>(elements.childCount);
                List<T> newValue = new List<T>(elements.childCount);
                foreach (VisualElement element in elements.Children())
                {
                    VisualElement field = element.ElementAt(1);
                    if (field == evt.target)
                    {
                        previousValue.Add(evt.previousValue);
                        newValue.Add(evt.newValue);
                    }
                    else
                    {
                        T value = ((INotifyValueChanged<T>)field).value;
                        previousValue.Add(value);
                        newValue.Add(value);
                    }
                }
                // Send the event
                using ChangeEvent<List<T>> change = ChangeEvent<List<T>>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            });
            field.AddToClassList(ElementField);

            // Register Events
            elementWrapper.RegisterCallback<MouseDownEvent>(evt =>
            {
                elementWrapper.Focus();
                _selectedElement = elementWrapper;
            });
            elementWrapper.RegisterCallback<BlurEvent>(evt =>
            {
                if (_selectedElement == elementWrapper) _selectedElement = null;
            });

            handle.RegisterCallback<MouseOverEvent>(evt => _mouseOverElement = elementWrapper);
            handle.RegisterCallback<MouseDownEvent>(evt =>
            {
                _dragElement = elementWrapper;
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.StartDrag("Drag List Element");
            });
            handle.RegisterCallback<MouseUpEvent>(evt =>
            {
                if (_dragElement == elementWrapper) _dragElement = null;
            });

            elementWrapper.Add(handle);
            elementWrapper.Add(field);
            return elementWrapper;
        }

        /// <returns>A field for type T</returns>
        protected abstract VisualElement CreateField();

        /// <summary>Updates the displayed indices of each element.</summary>
        private void UpdateIndices()
        {
            VisualElement elements = this.Q<VisualElement>(Elements);
            int count = 0;
            foreach(VisualElement element in elements.Children())
            {
                Label label = (Label)element.ElementAt(0);
                label.text = ShowIndices ? count.ToString() : null;
                count++;
            }
        }
        #endregion

        #region UXML Factory
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _title =
                new UxmlStringAttributeDescription { name = "title", defaultValue = null };
            private readonly UxmlBoolAttributeDescription _showIndices =
                new UxmlBoolAttributeDescription { name = "show-indices", defaultValue = false };
            private readonly UxmlBoolAttributeDescription _isAdditive =
                new UxmlBoolAttributeDescription { name = "additive", defaultValue = true };
            private readonly UxmlBoolAttributeDescription _isArrangeable =
                new UxmlBoolAttributeDescription { name = "arrangeable", defaultValue = true };
            private readonly UxmlBoolAttributeDescription _isCollapsible =
                new UxmlBoolAttributeDescription { name = "collapsible", defaultValue = true };
            private readonly UxmlBoolAttributeDescription _isExpanded =
                new UxmlBoolAttributeDescription { name = "expanded", defaultValue = true };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext ctx)
            {
                base.Init(element, bag, ctx);
                ListField<T> field = (ListField<T>)element;

                field.Title = _title.GetValueFromBag(bag, ctx);
                field.ShowIndices = _showIndices.GetValueFromBag(bag, ctx);
                field.IsAdditive = _isAdditive.GetValueFromBag(bag, ctx);
                field.IsArrangeable = _isArrangeable.GetValueFromBag(bag, ctx);
                field.IsCollapsible = _isCollapsible.GetValueFromBag(bag, ctx);
                field.IsExpanded = _isExpanded.GetValueFromBag(bag, ctx);
            }
        }
        #endregion
    }
}
