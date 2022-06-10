using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransPunchAnchorPosNode : TweenNode
    {
        public RTransPunchAnchorPosNode(RTransPunchAnchorPosVertex vertex) : base(vertex)
        {
            title = "Rect Trans Punch Anchor Pos Node";
            CreateNodePort("Punch", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Elasticity", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
