using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class SpriteBlendColorNode : TweenNode
    {
        public SpriteBlendColorNode(SpriteBlendColorVertex vertex) : base(vertex)
        {
            title = "Sprite Blend Color Node";
            CreateNodePort("Color", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
