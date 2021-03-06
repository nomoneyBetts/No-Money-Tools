using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class CameraPixelRectNode : TweenNode
    {
        public CameraPixelRectNode(CameraPixelRectVertex vertex) : base(vertex)
        {
            title = "Pixel Rect Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Rect));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Rect));
        }
    }
}
