// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEditor.Experimental.GraphView;
using DG.Tweening;


namespace NoMoney.DTAnimation
{
    public class LocalRotateNode : TweenNode
    {
        public LocalRotateNode(LocalRotateVertex vertex) : base(vertex)
        {
            title = "Rotate Node";

            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Rotate Mode", Orientation.Horizontal, Direction.Input, typeof(RotateMode));
        }
    }
}
