// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RBLookAtNode : TweenNode
    {
        public RBLookAtNode(RBLookAtVertex vertex) : base(vertex)
        {
            title = "RB Look-At Node";
            CreateNodePort("Towards", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Axis Constraint", Orientation.Horizontal, Direction.Input, typeof(AxisConstraint));
            CreateNodePort("Up Direction", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
