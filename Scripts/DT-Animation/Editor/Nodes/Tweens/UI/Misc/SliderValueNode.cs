using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class SliderValueNode : TweenNode
    {
        public SliderValueNode(SliderValueVertex vertex) : base(vertex)
        {
            title = "Slider Value Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
