using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ImageFillNode : TweenNode
    {
        public ImageFillNode(ImageFillVertex vertex) : base(vertex)
        {
            title = "Image Fill Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
