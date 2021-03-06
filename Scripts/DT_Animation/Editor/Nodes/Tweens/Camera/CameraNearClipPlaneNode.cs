using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CameraNearClipPlaneNode : TweenNode
    {
        public CameraNearClipPlaneNode(CameraNearClipPlaneVertex vertex) : base(vertex)
        {
            title = "Near Clip Plane Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
