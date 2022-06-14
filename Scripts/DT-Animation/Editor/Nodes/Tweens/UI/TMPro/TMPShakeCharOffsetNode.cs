// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPShakeCharOffsetNode : TweenNode
    {
        public TMPShakeCharOffsetNode(TMPShakeCharOffsetVertex vertex) : base(vertex)
        {
            title = "TMPro Shake Char Offset Node";
            CreateNodePort("Shake", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Randomness", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Char Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("TMPro Wrapper", Orientation.Horizontal, Direction.Input, typeof(object));
            CreateNodePort("Fade Out", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
