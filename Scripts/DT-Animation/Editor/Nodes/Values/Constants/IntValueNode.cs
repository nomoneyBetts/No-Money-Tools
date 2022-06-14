// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class IntValueNode : ValueNode<int>
    {
        public IntValueNode(ValueVertex<int> vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<int> vertex = (ValueVertex<int>)Vertex;
            IntegerField field = new IntegerField();
            field.SetValueWithoutNotify(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
