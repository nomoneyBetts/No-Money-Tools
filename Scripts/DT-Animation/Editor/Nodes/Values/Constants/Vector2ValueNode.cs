// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class Vector2ValueNode : ValueNode<Vector2>
    {
        public Vector2ValueNode(Vector2ValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Vector2> vertex = (ValueVertex<Vector2>)Vertex;
            Vector2Field field = new Vector2Field();
            field.SetValueWithoutNotify(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
