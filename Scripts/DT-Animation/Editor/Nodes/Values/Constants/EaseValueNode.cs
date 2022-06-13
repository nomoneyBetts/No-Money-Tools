using DG.Tweening;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class EaseValueNode : ValueNode<Ease>
    {
        public EaseValueNode(EaseValueVertex vertex) : base(vertex) { }

        protected override VisualElement CreateField()
        {
            ValueVertex<Ease> easeVertex = (ValueVertex<Ease>)Vertex;
            EnumField field = new EnumField(easeVertex.Value);
            field.RegisterValueChangedCallback(evt => easeVertex.Value = (Ease)evt.newValue);
            outputContainer.Add(field);
            return field;
        }
    }
}
