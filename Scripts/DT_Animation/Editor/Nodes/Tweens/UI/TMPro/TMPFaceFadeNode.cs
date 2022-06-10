using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPFaceFadeNode : TweenNode
    {
        public TMPFaceFadeNode(TMPFaceFadeVertex vertex) : base(vertex)
        {
            title = "TMPro Face Fade Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
