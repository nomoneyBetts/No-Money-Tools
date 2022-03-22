using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class MoveNode : TweenNode
    {
        public MoveNode(MoveVertex movVertex) : base(movVertex)
        {
            title = "Move Node";

            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
