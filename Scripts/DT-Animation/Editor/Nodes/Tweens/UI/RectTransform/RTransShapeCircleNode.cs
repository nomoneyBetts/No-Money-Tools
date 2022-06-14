// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransShapeCircleNode : TweenNode
    {
        public RTransShapeCircleNode(RTransShapeCircleVertex vertex) : base(vertex)
        {
            title = "Rect Trans Shake Anchor Pos Node";
            CreateNodePort("Center", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("End Value Deg", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
            CreateNodePort("Relative Center", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
