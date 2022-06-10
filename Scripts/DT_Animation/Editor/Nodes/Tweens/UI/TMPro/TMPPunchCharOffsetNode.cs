using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class TMPPunchCharOffsetNode : TweenNode
    {
        public TMPPunchCharOffsetNode(TMPPunchCharOffsetVertex vertex) : base(vertex)
        {
            title = "TMPro Punch Char Offset Node";
            CreateNodePort("Punch", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Elasticity", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Char Index", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("TMPro Wrapper", Orientation.Horizontal, Direction.Input, typeof(object));
        }
    }
}
