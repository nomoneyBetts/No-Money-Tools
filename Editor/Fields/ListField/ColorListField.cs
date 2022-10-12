// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoMoney.Editor
{
    /// <summary>
    /// A field for inputing a list of colors.
    /// </summary>
    public class ColorListField : ListField<Color>
    {
        public new class UxmlFactory : UxmlFactory<ColorListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new ColorField();
    }
}
