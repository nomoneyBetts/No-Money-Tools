// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    public class StringValueNode : ValueNode<string>
    {
        public StringValueNode(StringValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<string> vertex = (ValueVertex<string>)Vertex;
            TextField field = new TextField();
            field.SetValueWithoutNotify(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
