using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ScrollRectHorizontalPosNode : TweenNode
    {
        public ScrollRectHorizontalPosNode(ScrollRectHorizontalPosVertex vertex) : base(vertex)
        {
            title = "Scroll Rect Horizontal Position Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
