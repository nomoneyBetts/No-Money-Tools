using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class Vector3ValueNode : ValueNode<Vector3>
    {
        public Vector3ValueNode(Vector3ValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Vector3> vector3Vertex = (ValueVertex<Vector3>)Vertex;
            Vector3Field field = new Vector3Field();
            field.SetValueWithoutNotify(vector3Vertex.Value);
            field.RegisterValueChangedCallback(evt => vector3Vertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
