using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class BlendableLocalRotateNode : TweenNode
    {
        public BlendableLocalRotateNode(BlendableLocalRotateVertex vertex) : base(vertex)
        {
            title = "Blendable Local Rotate Node";
            CreateNodePort("By", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Rotate Mode", Orientation.Horizontal, Direction.Input, typeof(RotateMode));
        }
    }
}
