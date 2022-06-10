using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class LightBlendableColorNode : TweenNode
    {
        public LightBlendableColorNode(LightBlendableColorVertex vertex) : base(vertex)
        {
            title = "Light Blend Color Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
