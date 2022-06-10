using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPScaleNode : TweenNode
    {
        public TMPScaleNode(TMPScaleVertex vertex) : base(vertex)
        {
            title = "TMPro Scale Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
