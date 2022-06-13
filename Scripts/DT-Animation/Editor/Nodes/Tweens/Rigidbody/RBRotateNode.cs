using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    public class RBRotateNode : TweenNode
    {
        public RBRotateNode(RBRotateVertex vertex) : base(vertex)
        {
            title = "RB Rotate Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Rotate Mode", Orientation.Horizontal, Direction.Input, typeof(RotateMode));
        }
    }
}
