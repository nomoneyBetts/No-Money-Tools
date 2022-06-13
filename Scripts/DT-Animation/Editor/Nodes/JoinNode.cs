using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class JoinNode : DTAnimatorNode
    {
        public JoinNode(JoinVertex joinVertex) : base(joinVertex)
        {
            title = "Join Node";
            CreateNodePort("Join", Orientation.Horizontal, Direction.Output, typeof(Node), Port.Capacity.Multi);
            CreateNodePort("Input", Orientation.Horizontal, Direction.Input, typeof(Node), Port.Capacity.Multi);
        }
    }
}
