using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPFontSizeNode : TweenNode
    {
        public TMPFontSizeNode(TMPFontSizeVertex vertex) : base(vertex)
        {
            title = "TMPro Font Size Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
