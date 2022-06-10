using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of Vector 4's.
    /// </summary>
    public class Vector4ListField : ListField<Vector4>
    {
        public new class UxmlFactory : UxmlFactory<Vector4ListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new Vector4Field();
    }
}
