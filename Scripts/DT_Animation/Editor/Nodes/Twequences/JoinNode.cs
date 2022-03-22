using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class JoinNode : TwequenceNode
    {
        public JoinNode(JoinVertex joinVertex) : base(joinVertex)
        {
            title = "Join Node";
            CreateNodePort("Join", Orientation.Horizontal, Direction.Output, typeof(float), Port.Capacity.Multi);
        }
    }
}
