using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TrailTimeNode : TweenNode
    {
        public TrailTimeNode(TrailTimeVertex vertex) : base(vertex)
        {
            title = "Trail Time Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
