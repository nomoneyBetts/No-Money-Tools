using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CameraOrthoSizeNode : TweenNode
    {
        public CameraOrthoSizeNode(CameraOrthoSizeVertex vertex) : base(vertex)
        {
            title = "Ortho Size Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
