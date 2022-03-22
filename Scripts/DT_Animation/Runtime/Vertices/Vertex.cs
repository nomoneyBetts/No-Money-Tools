using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public abstract class Vertex : ScriptableObject
    {
        public Vector2 NodePosition;

        public void DestroyVertex()
        {
            DestroyImmediate(this);
        }

        /// <summary>
        /// Connects to a vertex through the port.
        /// </summary>
        /// <param name="portName">Name of port to connect through.</param>
        /// <param name="id">Connecting Vertex's ID.</param>
        /// <returns>True on succesfull connection.</returns>
        public abstract void ConnectVertex(string portName, string cnxPort, Vertex vertex);

        /// <summary>
        /// Disconnects from a vertex through the port.
        /// </summary>
        /// <param name="portName">Name of port to disconnect through.</param>
        /// <param name="id">Disconnecting Vertex's ID.</param>
        /// <returns>True on succesfull disconnection.</returns>
        public abstract void DisconnectVertex(string portName, string cnxPort, Vertex vertex);

        /// <returns>A list of Output connections.</returns>
        public abstract List<Connection> GetOutputs();

        /// <returns>A list of Input connections.</returns>
        public abstract List<Connection> GetInputs();
    }
}
