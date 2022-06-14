// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;

namespace NoMoney.Audio
{
    public class DropdownListField : ListField<string>
    {
        public new class UxmlFactory : UxmlFactory<DropdownListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new DropdownElement();
    }
}
