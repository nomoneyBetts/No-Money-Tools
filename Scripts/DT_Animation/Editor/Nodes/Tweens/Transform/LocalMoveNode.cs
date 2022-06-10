using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class LocalMoveNode : TweenNode
    {
        public LocalMoveNode(LocalMoveVertex vertex) : base(vertex)
        {
            title = "Local Move Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
