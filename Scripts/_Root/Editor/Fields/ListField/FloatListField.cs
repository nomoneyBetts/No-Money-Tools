using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of floats.
    /// </summary>
    public class FloatListField : ListField<float>
    {
        public new class UxmlFactory : UxmlFactory<FloatListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new FloatField();
    }
}
