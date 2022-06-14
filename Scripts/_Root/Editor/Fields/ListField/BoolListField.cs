// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of bools.
    /// </summary>
    public class BoolListField : ListField<bool>
    {
        public new class UxmlFactory : UxmlFactory<BoolListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new Toggle();
    }
}
