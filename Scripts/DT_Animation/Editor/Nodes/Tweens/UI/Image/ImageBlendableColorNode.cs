using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class ImageBlendableColorNode : TweenNode
    {
        public ImageBlendableColorNode(ImageBlendableColorVertex vertex) : base(vertex)
        {
            title = "Image Gradient Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
