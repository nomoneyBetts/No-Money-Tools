using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CameraAspectNode : TweenNode
    {
        public CameraAspectNode(CameraAspectVertex vertex) : base(vertex)
        {
            title = "Aspect Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
