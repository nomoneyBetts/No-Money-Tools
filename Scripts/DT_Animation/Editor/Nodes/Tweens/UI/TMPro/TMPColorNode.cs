using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPColorNode : TweenNode
    {
        public TMPColorNode(TMPColorVertex vertex) : base(vertex)
        {
            title = "TMPro Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
