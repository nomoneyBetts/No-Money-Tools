using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    public class PathNode : TweenNode
    {
        public PathNode(PathVertex vertex) : base(vertex)
        {
            title = "Path Node";

            CreateNodePort("Path Curve", Orientation.Horizontal, Direction.Input, typeof(PathCurve));
            CreateNodePort("Path Mode", Orientation.Horizontal, Direction.Input, typeof(PathMode));
            CreateNodePort("Pos Axis Constraint", Orientation.Horizontal, Direction.Input, typeof(AxisConstraint));
            CreateNodePort("Rot Axis Constraint", Orientation.Horizontal, Direction.Input, typeof(AxisConstraint));
            CreateNodePort("Look-At Target", Orientation.Horizontal, Direction.Input, typeof(Transform));
            CreateNodePort("Look-At Position", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Look Ahead", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Forward", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Up", Orientation.Horizontal, Direction.Input, typeof(Vector3));
        }
    }
}
