using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class MaterialColorNode : TweenNode
    {
        public MaterialColorNode(MaterialColorVertex vertex) : base(vertex)
        {
            title = "Material Color Node";
            CreateNodePort("Material Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("Property Name", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Property ID", Orientation.Horizontal, Direction.Input, typeof(int));
        }
    }
}
