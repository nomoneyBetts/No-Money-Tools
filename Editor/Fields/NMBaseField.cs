// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;

namespace NoMoney.Editor
{
    /// <summary>
    /// Base class for custom fields.
    /// </summary>
    /// <typeparam name="T">The type of field.</typeparam>
    public abstract class NMBaseField<T> : VisualElement, INotifyValueChanged<T>
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

        public abstract T value { get; set; }

        public NMBaseField()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("NMBaseField"));
        }

        public abstract void SetValueWithoutNotify(T newValue);
    }
}
