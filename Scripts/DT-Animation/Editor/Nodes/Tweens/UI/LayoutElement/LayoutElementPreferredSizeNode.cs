using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class LayoutElementPreferredSizeNode : TweenNode
    {
        public LayoutElementPreferredSizeNode(LayoutElementPreferredSizeVertex vertex) : base(vertex)
        {
            title = "Layout Element Preferred Size Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
