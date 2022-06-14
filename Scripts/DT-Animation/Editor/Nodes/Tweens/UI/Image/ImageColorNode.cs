// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class ImageColorNode : TweenNode
    {
        public ImageColorNode(ImageColorVertex vertex) : base(vertex)
        {
            title = "Image Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
