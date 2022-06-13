using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class BlendableMoveNode : TweenNode
    {
        public BlendableMoveNode(BlendableMoveVertex vertex) : base(vertex)
        {
            title = "Blendable Move Node";
            CreateNodePort("By", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
