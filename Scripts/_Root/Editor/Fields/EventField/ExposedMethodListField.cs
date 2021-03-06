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
