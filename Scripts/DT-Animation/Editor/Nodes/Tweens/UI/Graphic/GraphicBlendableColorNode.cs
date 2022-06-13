using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class GraphicBlendableColorNode : TweenNode
    {
        public GraphicBlendableColorNode(GraphicBlendableColorVertex vertex) : base(vertex)
        {
            title = "Graphic Blendable Color Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
