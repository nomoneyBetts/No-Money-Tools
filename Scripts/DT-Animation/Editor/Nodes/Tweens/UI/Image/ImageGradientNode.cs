// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class ImageGradientNode : TweenNode
    {
        public ImageGradientNode(ImageGradientVertex vertex) : base(vertex)
        {
            title = "Image Gradient Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Gradient));
        }
    }
}
