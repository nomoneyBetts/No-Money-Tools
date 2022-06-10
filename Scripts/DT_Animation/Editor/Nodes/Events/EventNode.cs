using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public abstract class EventNode : DTAnimatorNode
    {
        public EventNode(EventVertex vertex) : base(vertex)
        {
            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(ExposedEvent), Port.Capacity.Multi);
            EventField field = new EventField();
            field.SetValueWithoutNotify(vertex.ExposedEvent);
            field.RegisterValueChangedCallback(evt => vertex.ExposedEvent = evt.newValue);
            extensionContainer.Add(field);
            RefreshExpandedState();
        }
    }
}
