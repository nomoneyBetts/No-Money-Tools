using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ScrollRectNormalizedPosNode : TweenNode
    {
        public ScrollRectNormalizedPosNode(ScrollRectNormalizedPosVertex vertex) : base(vertex)
        {
            title = "Scroll Rect Normalized Position Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
