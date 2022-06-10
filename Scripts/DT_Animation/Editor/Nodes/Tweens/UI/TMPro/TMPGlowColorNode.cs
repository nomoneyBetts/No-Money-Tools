using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPGlowColorNode : TweenNode
    {
        public TMPGlowColorNode(TMPGlowColorVertex vertex) : base(vertex)
        {
            title = "TMPro Glow Color Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("Shared Material", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
