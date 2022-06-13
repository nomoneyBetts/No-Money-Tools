using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class GraphicColorNode : TweenNode
    {
        public GraphicColorNode(GraphicColorVertex vertex) : base(vertex)
        {
            title = "Graphic Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
