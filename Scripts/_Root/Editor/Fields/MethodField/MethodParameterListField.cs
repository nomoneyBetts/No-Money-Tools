using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing MethodParameters.
    /// </summary>
    internal class MethodParameterListField : ListField<MethodParameter>
    {
        public new class UxmlFactory : UxmlFactory<MethodParameterListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new MethodParameterElement();
    }
}
