// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.Editor
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
