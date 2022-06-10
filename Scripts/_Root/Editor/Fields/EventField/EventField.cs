using System;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing ExposedEvents.
    /// </summary>
    public class EventField : NoMoneyField<ExposedEvent>
    {
        // idk why, but this doesn't work.
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

        protected override ExposedEvent GetValue()
        {
            return new ExposedEvent
            {
                Methods = this.Q<ExposedMethodListField>().value
            };
        }

        protected override void SetValue(ExposedEvent value)
        {
            this.Q<ExposedMethodListField>().value = value.Methods;
        }

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
