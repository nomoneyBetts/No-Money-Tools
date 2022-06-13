using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class ShakeScaleNode : TweenNode
    {
        public ShakeScaleNode(ShakeScaleVertex vertex) : base(vertex)
        {
            title = "Shake Scale Node";
            CreateNodePort("Strength", Orientation.Horizontal, Direction.Input, typeof(Vector3));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Randomness", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Fade Out", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
