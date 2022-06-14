// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RBMoveNode : TweenNode
    {
        public RBMoveNode(RBMoveVertex vertex) : base(vertex)
        {
            title = "RB Move Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
