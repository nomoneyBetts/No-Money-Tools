// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.Editor
{
    /// <summary>
    /// A field for inputing a list of Vector 3's.
    /// </summary>
    public class Vector3ListField : ListField<Vector3>
    {
        public new class UxmlFactory : UxmlFactory<Vector3ListField, UxmlTraits> { }

        protected override VisualElement CreateField() => new Vector3Field();
    }
}
