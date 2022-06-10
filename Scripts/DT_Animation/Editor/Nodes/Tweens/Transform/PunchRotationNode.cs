using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class PunchRotationNode : TweenNode
    {
        public PunchRotationNode(PunchRotationVertex vertex) : base(vertex)
        {
            title = "Punch Rotation Node";
            CreateNodePort("Punch", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Elasticity", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
