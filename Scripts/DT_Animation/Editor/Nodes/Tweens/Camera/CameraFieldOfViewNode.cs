using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CameraFieldOfViewNode : TweenNode
    {
        public CameraFieldOfViewNode(CameraFieldOfViewVertex vertex) : base(vertex)
        {
            title = "Field of View Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
