// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class TargetValueNode : ValueNode<Object>
    {
        public TargetValueNode(TargetValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Object> transVertex = (ValueVertex<Object>)Vertex;
            ObjectField field = new ObjectField();
            field.objectType = typeof(Object);
            field.SetValueWithoutNotify(transVertex.Value);
            field.RegisterValueChangedCallback(evt => transVertex.Value = (Object)evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
