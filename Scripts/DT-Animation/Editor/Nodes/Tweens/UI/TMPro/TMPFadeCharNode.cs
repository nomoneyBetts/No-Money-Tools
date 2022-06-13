using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPFadeCharNode : TweenNode
    {
        public TMPFadeCharNode(TMPFadeCharVertex vertex) : base(vertex)
        {
            title = "TMPro Fade Char Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Char Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("TMPro Wrapper", Orientation.Horizontal, Direction.Input, typeof(object));
        }
    }
}
