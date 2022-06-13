using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class BlendableScaleNode : TweenNode
    {
        public BlendableScaleNode(BlendableScaleVertex vertex) : base(vertex)
        {
            title = "Blendable Scale Node";
            CreateNodePort("By", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
