using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransSizeDeltaNode : TweenNode
    {
        public RTransSizeDeltaNode(RTransSizeDeltaVertex vertex) : base(vertex)
        {
            title = "Rect Trans Shake Anchor Pos Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
