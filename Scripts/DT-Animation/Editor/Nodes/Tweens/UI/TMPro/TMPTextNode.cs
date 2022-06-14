// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPTextNode : TweenNode
    {
        public TMPTextNode(TMPTextVertex vertex) : base(vertex)
        {
            title = "TMPro Outline Color Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Scramble Chars", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Scramble Mode", Orientation.Horizontal, Direction.Input, typeof(ScrambleMode));
            CreateNodePort("Rich Text", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
