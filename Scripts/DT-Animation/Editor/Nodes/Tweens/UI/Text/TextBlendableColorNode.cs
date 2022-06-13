using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TextBlendableColorNode : TweenNode
    {
        public TextBlendableColorNode(TextBlendableColorVertex vertex) : base(vertex)
        {
            title = "Text Blendable Color Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
