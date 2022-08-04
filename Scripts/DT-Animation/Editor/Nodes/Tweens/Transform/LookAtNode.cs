// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEditor.Experimental.GraphView;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    public class LookAtNode : TweenNode
    {
        public LookAtNode(LookAtVertex vertex) : base(vertex)
        {
            title = "Look-At Node";
            CreateNodePort("Towards", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Axis Constraint", Orientation.Horizontal, Direction.Input, typeof(AxisConstraint));
            CreateNodePort("Up Direction", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
