// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPPunchCharRotationNode : TweenNode
    {
        public TMPPunchCharRotationNode(TMPPunchCharRotationVertex vertex) : base(vertex)
        {
            title = "TMPro Punch Char Rotation Node";
            CreateNodePort("Punch", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Elasticity", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Char Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("TMPro Wrapper", Orientation.Horizontal, Direction.Input, typeof(object));
        }
    }
}
