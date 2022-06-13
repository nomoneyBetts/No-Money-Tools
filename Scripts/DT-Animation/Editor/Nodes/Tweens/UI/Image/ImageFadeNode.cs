using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ImageFadeNode : TweenNode
    {
        public ImageFadeNode(ImageFadeVertex vertex) : base(vertex)
        {
            title = "Image Fade Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
