using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ScrollRectVerticalPosNode : TweenNode
    {
        public ScrollRectVerticalPosNode(ScrollRectVerticalPosVertex vertex) : base(vertex)
        {
            title = "Scroll Rect Vertical Position Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
