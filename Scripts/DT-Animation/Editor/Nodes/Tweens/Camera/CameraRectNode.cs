using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class CameraRectNode : TweenNode
    {
        public CameraRectNode(CameraRectVertex vertex) : base(vertex)
        {
            title = "Camera Rect Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Rect));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Rect));
        }
    }
}
