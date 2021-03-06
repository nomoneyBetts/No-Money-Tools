using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CanvasGroupFadeNode : TweenNode
    {
        public CanvasGroupFadeNode(CanvasGroupFadeVertex vertex) : base(vertex)
        {
            title = "Canvas Group Fade Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
