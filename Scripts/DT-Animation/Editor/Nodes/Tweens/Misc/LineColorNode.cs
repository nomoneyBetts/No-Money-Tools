using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class LineColorNode : TweenNode
    {
        public LineColorNode(LineColorVertex vertex) : base(vertex)
        {
            title = "Line Color Node";
            CreateNodePort("A) Start Color", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("A) End Color", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("B) Start Color", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("B) End Color", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
