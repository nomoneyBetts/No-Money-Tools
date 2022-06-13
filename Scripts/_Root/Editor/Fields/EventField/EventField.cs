using System;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing ExposedEvents.
    /// </summary>
    public class EventField : NMBaseField<ExposedEvent>
    {
        // TODO: Solve the case of EventField Parameter Types.
        //idk why, but this doesn't work.
        //private Type[] _parameterTypes;
        //public Type[] ParameterTypes
        //{
        //    get => _parameterTypes;
        //    set
        //    {
        //        _parameterTypes = value;
        //        UpdateParameters(value);
        //    }
        //}

        public override ExposedEvent value
        {
            get => new ExposedEvent
            {
                Methods = this.Q<ExposedMethodListField>().value
            };
            set => this.Q<ExposedMethodListField>().value = value.Methods;
        }

        public EventField()
        {
            ExposedMethodListField field = new ExposedMethodListField { Title = "Methods" };
            field.RegisterValueChangedCallback(evt =>
            {
                evt.StopPropagation();
                ExposedEvent previousValue = new ExposedEvent { Methods = evt.previousValue };
                ExposedEvent newValue = new ExposedEvent { Methods = evt.newValue };
                //UpdateParameters(ParameterTypes);

                using ChangeEvent<ExposedEvent> change = ChangeEvent<ExposedEvent>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            });

            Add(field);
        }

        public EventField(string title) : this() => Title = title;

        public override void SetValueWithoutNotify(ExposedEvent newValue)
        {
            ExposedMethodListField listField = this.Q<ExposedMethodListField>();
            if (newValue == null) listField.SetValueWithoutNotify(null);
            else listField.SetValueWithoutNotify(newValue.Methods);
        }

        /// <summary>
        /// Update all the required parameter types in each of the exposed methods.
        /// </summary>
        /// <param name="types"></param>
        private void UpdateParameters(Type[] types)
        {
            ExposedMethodListField listField = this.Q<ExposedMethodListField>();
            List<MethodField> fields = listField.GetFields<MethodField>();
            foreach(MethodField field in fields)
            {
                field.ParameterTypes = types;
            }
        }

        #region UXML Factory
        public new class UxmlFactory : UxmlFactory<EventField, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _title =
                new UxmlStringAttributeDescription { name = "title", defaultValue = null };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext ctx)
            {
                base.Init(element, bag, ctx);
                EventField field = (EventField)element;

                field.Title = _title.GetValueFromBag(bag, ctx);
            }
        }
        #endregion
    }
}
