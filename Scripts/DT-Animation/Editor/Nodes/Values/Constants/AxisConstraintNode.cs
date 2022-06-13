using DG.Tweening;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class AxisConstraintNode : ValueNode<AxisConstraint>
    {
        public AxisConstraintNode(AxisConstraintVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<AxisConstraint> vertex = (ValueVertex<AxisConstraint>)Vertex;
            EnumField field = new EnumField(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = (AxisConstraint)evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
