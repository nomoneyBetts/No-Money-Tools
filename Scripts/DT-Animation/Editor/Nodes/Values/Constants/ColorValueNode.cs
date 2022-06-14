// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class ColorValueNode : ValueNode<Color>
    {
        public ColorValueNode(ColorValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Color> colorVertex = (ValueVertex<Color>)Vertex;
            ColorField field = new ColorField();
            field.SetValueWithoutNotify(colorVertex.Value);
            field.RegisterValueChangedCallback(evt => colorVertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
