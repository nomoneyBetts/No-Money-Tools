using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class SpriteColorNode : TweenNode
    {
        public SpriteColorNode(SpriteColorVertex vertex) : base(vertex)
        {
            title = "Sprite Color Node";

            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
        }
    }
}
