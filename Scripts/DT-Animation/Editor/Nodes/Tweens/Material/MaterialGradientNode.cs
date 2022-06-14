// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class MaterialGradientNode : TweenNode
    {
        public MaterialGradientNode(MaterialGradientVertex vertex) : base(vertex)
        {
            title = "Material Gradient Node";
            CreateNodePort("Material Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Gradient));
            CreateNodePort("Property Name", Orientation.Horizontal, Direction.Input, typeof(string));
        }
    }
}
