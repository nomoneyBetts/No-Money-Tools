// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class MaterialBlendableColorNode : TweenNode
    {
        public MaterialBlendableColorNode(MaterialBlendableColorVertex vertex) : base(vertex)
        {
            title = "Material Blendable Color Node";
            CreateNodePort("Material Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("Property Name", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Property ID", Orientation.Horizontal, Direction.Input, typeof(int));
        }
    }
}
