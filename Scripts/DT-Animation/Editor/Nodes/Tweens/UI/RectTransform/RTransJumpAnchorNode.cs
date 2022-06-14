// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransJumpAnchorNode : TweenNode
    {
        public RTransJumpAnchorNode(RTransJumpAnchorVertex vertex) : base(vertex)
        {
            title = "Rect Trans Anchor Jump Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Jump Power", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Num Jumps", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
