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
    /// A field for inputing a list of Vector 2's.
    /// </summary>
    public class Vector2ListField : ListField<Vector2>
    {
        public new class UxmlFactory : UxmlFactory<Vector2ListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new Vector2Field();
    }
}
