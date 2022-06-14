// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPColorCharNode : TweenNode
    {
        public TMPColorCharNode(TMPColorCharVertex vertex) : base(vertex)
        {
            title = "TMPro Color Char Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Color));
            CreateNodePort("Char Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("TMPro Wrapper", Orientation.Horizontal, Direction.Input, typeof(object));
        }
    }
}
