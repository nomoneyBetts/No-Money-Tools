using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class LightIntensityNode : TweenNode
    {
        public LightIntensityNode(LightIntensityVertex vertex) : base(vertex)
        {
            title = "Light Intensity Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
