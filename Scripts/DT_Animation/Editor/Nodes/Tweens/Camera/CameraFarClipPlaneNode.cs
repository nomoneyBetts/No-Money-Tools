using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CameraFarClipPlaneNode : TweenNode
    {
        public CameraFarClipPlaneNode(CameraFarClipPlaneVertex vertex) : base(vertex)
        {
            title = "Far Clip Plane Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
