using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class MaterialFloatNode : TweenNode
    {
        public MaterialFloatNode(MaterialFloatVertex vertex) : base(vertex)
        {
            title = "Material Float Node";
            CreateNodePort("Material Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Property Name", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Property ID", Orientation.Horizontal, Direction.Input, typeof(int));
        }
    }
}
