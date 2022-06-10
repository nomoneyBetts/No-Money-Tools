using UnityEngine;
using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// Base class for custom fields.
    /// </summary>
    /// <typeparam name="T">The type of field.</typeparam>
    public abstract class NoMoneyField<T> : VisualElement, INotifyValueChanged<T>
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (value != null)
                {
                    if (this[0] is Label label) label.text = value;
                    else Insert(0, new Label(value) { name = "field-title" });
                }
                else if (this[0] is Label label) Remove(label);
                _title = value;
            }
        }

        public T value { get => GetValue(); set => SetValue(value); }

        public NoMoneyField()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("NoMoneyField"));
        }

        public abstract void SetValueWithoutNotify(T newValue);

        /// <returns>The value of the field.</returns>
        protected abstract T GetValue();

        /// <summary>Sets the value of the field.</summary>
        protected abstract void SetValue(T value);
    }
}
