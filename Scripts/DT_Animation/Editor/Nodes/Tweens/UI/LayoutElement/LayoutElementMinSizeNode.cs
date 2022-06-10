using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class LayoutElementMinSizeNode : TweenNode
    {
        public LayoutElementMinSizeNode(LayoutElementMinSizeVertex vertex) : base(vertex)
        {
            title = "Layout Element Min Size Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
