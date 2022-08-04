// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ShakeRotationNode : TweenNode
    {
        public ShakeRotationNode(ShakeRotationVertex vertex) : base(vertex)
        {
            title = "Shake Rotation Node";
            CreateNodePort("Strength", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Randomness", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Fade Out", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
