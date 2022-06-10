using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class BlendableLocalMoveNode : TweenNode
    {
        public BlendableLocalMoveNode(BlendableLocalMoveVertex vertex) : base(vertex)
        {
            title = "Blendable Local Move Node";
            CreateNodePort("By", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
