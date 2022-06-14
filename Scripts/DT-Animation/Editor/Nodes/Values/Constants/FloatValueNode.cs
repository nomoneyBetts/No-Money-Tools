// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class FloatValueNode : ValueNode<float>
    {
        public FloatValueNode(FloatValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<float> floatVertex = (ValueVertex<float>)Vertex;
            FloatField field = new FloatField();
            field.SetValueWithoutNotify(floatVertex.Value);
            field.RegisterValueChangedCallback(evt => floatVertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
