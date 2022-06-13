using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPOutlineColorNode : TweenNode
    {
        public TMPOutlineColorNode(TMPOutlineColorVertex vertex) : base(vertex)
        {
            title = "TMPro Outline Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
