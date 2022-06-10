using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransAnchorPos3DNode : TweenNode
    {
        public RTransAnchorPos3DNode(RTransAnchorPos3DVertex vertex) : base(vertex)
        {
            title = "Rect Trans Anchor Pos 3D Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
