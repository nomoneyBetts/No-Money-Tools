using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of colors.
    /// </summary>
    public class ColorListField : ListField<Color>
    {
        public new class UxmlFactory : UxmlFactory<ColorListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new ColorField();
    }
}
