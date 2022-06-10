using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TextColorNode : TweenNode
    {
        public TextColorNode(TextColorVertex vertex) : base(vertex)
        {
            title = "Text Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
