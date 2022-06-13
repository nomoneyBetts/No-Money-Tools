using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TextFadeNode : TweenNode
    {
        public TextFadeNode(TextFadeVertex vertex) : base(vertex)
        {
            title = "Text Fade Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
