// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class MaterialVectorNode : TweenNode
    {
        public MaterialVectorNode(MaterialVectorVertex vertex) : base(vertex)
        {
            title = "Material Vector Node";
            CreateNodePort("Material Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector4));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector4));
            CreateNodePort("Property Name", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Property ID", Orientation.Horizontal, Direction.Input, typeof(int));
        }
    }
}
