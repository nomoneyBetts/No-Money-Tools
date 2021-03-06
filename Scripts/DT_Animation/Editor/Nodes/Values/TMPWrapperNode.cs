using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    public class TMPWrapperNode : DTAnimatorNode
    {
        public TMPWrapperNode(TMPWrapperVertex vertex) : base(vertex)
        {
            this.Q<VisualElement>("title").RemoveFromHierarchy();
            AddToClassList("value-node");
            outputContainer.name = "value-output";

            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(Node), Port.Capacity.Multi);
            outputContainer.Add(new Label("TMPro Wrapper"));
        }
    }
}
