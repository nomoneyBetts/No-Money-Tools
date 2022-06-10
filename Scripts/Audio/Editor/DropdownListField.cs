using UnityEngine.UIElements;

namespace NoMoney.Audio
{
    public class DropdownListField : ListField<string>
    {
        public new class UxmlFactory : UxmlFactory<DropdownListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new DropdownElement();
    }
}
