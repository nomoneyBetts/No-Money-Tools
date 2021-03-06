using DG.Tweening;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class RotateModeValueNode : ValueNode<RotateMode>
    {
        public RotateModeValueNode(RotateModeValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<RotateMode> rotModeVertex = (ValueVertex<RotateMode>)Vertex;
            EnumField field = new EnumField(rotModeVertex.Value);
            field.RegisterValueChangedCallback(evt => rotModeVertex.Value = (RotateMode)evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
