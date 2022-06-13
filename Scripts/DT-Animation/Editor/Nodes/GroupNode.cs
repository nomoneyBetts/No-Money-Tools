using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class GroupNode : Group
    {
        public readonly GroupVertex Vertex;

        public GroupNode(GroupVertex vertex)
        {
            Vertex = vertex;
        }
    }
}
