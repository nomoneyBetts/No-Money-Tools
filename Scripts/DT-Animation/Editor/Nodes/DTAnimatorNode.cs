// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public abstract class DTAnimatorNode : Node
    {
        public readonly Vertex Vertex;

        public DTAnimatorNode(Vertex vertex)
        {
            Vertex = vertex;
            SetPosition(new Rect(vertex.NodePosition, Vector2.zero));
        }

        protected Port CreateNodePort(string name, Orientation orientation, Direction direction, Type type,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = InstantiatePort(orientation, direction, capacity, type);
            port.portName = name;
            if (direction == Direction.Input)
            {
                inputContainer.Add(port);
            }
            else
            {
                outputContainer.Add(port);
            }

            RefreshPorts();

            return port;
        }
    }
}
