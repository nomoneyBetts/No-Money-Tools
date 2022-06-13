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
