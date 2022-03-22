using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public abstract class TwequenceNode : DTAnimatorNode
    {
        public TwequenceNode(TwequenceVertex vertex) : base(vertex)
        {
            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(float));
        }
    }
}
