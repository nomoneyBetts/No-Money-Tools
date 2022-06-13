using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class Vector4ValueNode : ValueNode<Vector4>
    {
        public Vector4ValueNode(Vector4ValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Vector4> vertex = (ValueVertex<Vector4>)Vertex;
            Vector4Field field = new Vector4Field();
            field.SetValueWithoutNotify(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
