using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class OutlineColorNode : TweenNode
    {
        public OutlineColorNode(OutlineColorVertex vertex) : base(vertex)
        {
            title = "Outline Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
