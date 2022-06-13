using DG.Tweening;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class ScrambleModeValueNode : ValueNode<ScrambleMode>
    {
        public ScrambleModeValueNode(ScrambleModeValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<ScrambleMode> vertex = (ValueVertex<ScrambleMode>)Vertex;
            EnumField field = new EnumField(vertex.Value);
            field.RegisterValueChangedCallback(evt => vertex.Value = (ScrambleMode)evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
