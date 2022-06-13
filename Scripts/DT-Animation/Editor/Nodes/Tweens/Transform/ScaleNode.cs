using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ScaleNode : TweenNode
    {
        public ScaleNode(ScaleVertex vertex) : base(vertex)
        {
            title = "Scale Node";

            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
