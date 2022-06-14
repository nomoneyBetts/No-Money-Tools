// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

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
