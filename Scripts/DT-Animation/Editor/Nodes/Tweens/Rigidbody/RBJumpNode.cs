// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RBJumpNode : TweenNode
    {
        public RBJumpNode(RBJumpVertex vertex) : base(vertex)
        {
            title = "RB Jump Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Jump Power", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Num Jumps", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
