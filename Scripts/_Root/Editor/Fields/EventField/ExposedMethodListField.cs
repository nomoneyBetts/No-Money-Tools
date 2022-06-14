// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// A list for inputing ExposedMethods.
    /// </summary>
    internal class ExposedMethodListField : ListField<ExposedMethod>
    {
        protected override VisualElement CreateField() => new MethodField();
    }
}
