using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of Vector 3's.
    /// </summary>
    public class Vector3ListField : ListField<Vector3>
    {
        public new class UxmlFactory : UxmlFactory<Vector3ListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new Vector3Field();
    }
}
