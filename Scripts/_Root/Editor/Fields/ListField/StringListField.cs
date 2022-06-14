// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

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
