// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RBSpiralNode : TweenNode
    {
        public RBSpiralNode(RBSpiralVertex vertex) : base(vertex)
        {
            title = "RB Look-At Node";
            CreateNodePort("Axis", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Spiral Mode", Orientation.Horizontal, Direction.Input, typeof(SpiralMode));
            CreateNodePort("Speed", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Frequency", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Depth", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
