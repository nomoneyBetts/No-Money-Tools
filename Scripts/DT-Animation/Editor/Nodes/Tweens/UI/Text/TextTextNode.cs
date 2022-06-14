// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TextTextNode : TweenNode
    {
        public TextTextNode(TextTextVertex vertex) : base(vertex)
        {
            title = "Text Text Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Scramble Mode", Orientation.Horizontal, Direction.Input, typeof(ScrambleMode));
            CreateNodePort("Scramble Chars", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Rich Text", Orientation.Horizontal, Direction.Input, typeof(bool));

        }
    }
}
