using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    public class RTransPivotNode : TweenNode
    {
        public RTransPivotNode(RTransPivotVertex vertex) : base(vertex)
        {
            title = "Rect Trans Pivot Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(Vector2));
        }
    }
}
