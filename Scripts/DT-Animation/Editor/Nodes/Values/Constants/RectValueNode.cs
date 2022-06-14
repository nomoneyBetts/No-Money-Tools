// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class RectValueNode : ValueNode<Rect>
    {
        public RectValueNode(RectValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Rect> vertex = (ValueVertex<Rect>)Vertex;
            RectField field = new RectField();
            field.SetValueWithoutNotify(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
