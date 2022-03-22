using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public abstract class TweenNode : TwequenceNode
    {
        public TweenNode(TweenVertex tweVertex) : base(tweVertex)
        {
            CreateNodePort("Input", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Duration", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Delay", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Ease", Orientation.Horizontal, Direction.Input, typeof(Ease));
        }
    }
}
