// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class LightShadowStrengthNode : TweenNode
    {
        public LightShadowStrengthNode(LightShadowStrengthVertex vertex) : base(vertex)
        {
            title = "Light Shadow Strength Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
