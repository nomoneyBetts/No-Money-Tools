using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPFaceColorNode : TweenNode
    {
        public TMPFaceColorNode(TMPFaceColorVertex vertex) : base(vertex)
        {
            title = "TMPro Face Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
