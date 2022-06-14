// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    public class BoolValueNode : ValueNode<bool>
    {
        public BoolValueNode(BoolValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            BoolValueVertex vertex = (BoolValueVertex)Vertex;
            Toggle field = new Toggle();
            field.SetValueWithoutNotify(vertex.Value);
            field.RegisterValueChangedCallback(evt =>
            {
                vertex.Value = evt.newValue;
            });
            outputContainer.Add(field);
            return field;
        }
    }
}
