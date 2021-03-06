using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransAnchorMinNode : TweenNode
    {
        public RTransAnchorMinNode(RTransAnchorMinVertex vertex) : base(vertex)
        {
            title = "Rect Trans Anchor Min Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
