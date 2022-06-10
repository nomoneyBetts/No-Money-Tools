using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public abstract class Vertex : ScriptableObject
    {
        public Vector2 NodePosition;
        /// <summary>A list of all output connections.</summary>
        public abstract List<Connection> Outputs { get; }
        /// <summary>A list of all input connections.</summary>
        public abstract List<Connection> Inputs { get; }

        public void DestroyVertex()
        {
            DestroyImmediate(this);
        }

        /// <summary>
        /// Connects to a vertex through the port.
        /// </summary>
        /// <param name="portName">Name of port to connect through.</param>
        /// <param name="cnxPort">Connecting vertex's port.</param>
        /// <param name="vertex">The connecting vertex.</param>
        /// <returns>True on succesfull connection.</returns>
        public abstract bool ConnectVertex(string portName, string cnxPort, Vertex vertex);

        /// <summary>
        /// Disconnects from a vertex through the port.
        /// </summary>
        /// <param name="portName">Name of port to connect through.</param>
        /// <param name="cnxPort">Connecting vertex's port.</param>
        /// <param name="vertex">The connecting vertex.</param>
        /// <returns>True on succesfull disconnection</returns>
        public abstract bool DisconnectVertex(string portName, string cnxPort, Vertex vertex);
    }
}
