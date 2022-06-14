// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing a list of ints.
    /// </summary>
    public class IntegerListField : ListField<int>
    {
        public new class UxmlFactory : UxmlFactory<IntegerListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new IntegerField();
    }
}
