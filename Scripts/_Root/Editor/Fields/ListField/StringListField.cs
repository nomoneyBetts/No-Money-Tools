using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of strings.
    /// </summary>
    public class StringListField : ListField<string>
    {
        public new class UxmlFactory : UxmlFactory<StringListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new TextField();
    }
}
