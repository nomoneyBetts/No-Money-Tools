// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class AnimationCurveValueNode : ValueNode<AnimationCurve>
    {
        public AnimationCurveValueNode(AnimationCurveValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<AnimationCurve> curveVertex = (ValueVertex<AnimationCurve>)Vertex;
            CurveField field = new CurveField();
            field.SetValueWithoutNotify(curveVertex.Value);
            field.RegisterValueChangedCallback(evt => curveVertex.Value = evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
