using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class CameraColorNode : TweenNode
    {
        public CameraColorNode(CameraColorVertex vertex) : base(vertex)
        {
            title = "Camera Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
