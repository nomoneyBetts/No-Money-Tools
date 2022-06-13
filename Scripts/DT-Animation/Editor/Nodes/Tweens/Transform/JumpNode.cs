using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class JumpNode : TweenNode
    {
        public JumpNode(JumpVertex vertex) : base(vertex)
        {
            title = "Jump Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Jump Power", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Num Jumps", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
