using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class MaterialOffsetNode : TweenNode
    {
        public MaterialOffsetNode(MaterialOffsetVertex vertex) : base(vertex)
        {
            title = "Material Offset Node";
            CreateNodePort("Material Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Property Name", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Property ID", Orientation.Horizontal, Direction.Input, typeof(int));
        }
    }
}
