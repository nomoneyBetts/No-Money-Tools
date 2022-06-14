// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEditor.Experimental.GraphView;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    public class BlendableRotateNode : TweenNode
    {
        public BlendableRotateNode(BlendableRotateVertex vertex) : base(vertex)
        {
            title = "Blendable Rotate Node";
            CreateNodePort("By", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Rotate Mode", Orientation.Horizontal, Direction.Input, typeof(RotateMode));
        }
    }
}
