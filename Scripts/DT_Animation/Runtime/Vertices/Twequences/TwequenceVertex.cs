using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public abstract class TwequenceVertex : Vertex
    {
        public List<Connection> Outputs = new List<Connection>();
        public List<Connection> Inputs = new List<Connection>();

        public override void ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            Connection cnx = CreateInstance<Connection>();
            cnx.SetVals(portName, cnxPort, vertex);
            List<Connection> connections = IsOutput(portName) ? Outputs : Inputs;
            connections.Add(cnx);
        }

        public override void DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            List<Connection> connections = IsOutput(portName) ? Outputs : Inputs;
            Connection cnx = connections.Find(c => c.ValsMatch(portName, cnxPort, vertex));
            if (cnx != null)
            {
                cnx.DestroyCnx();
                connections.Remove(cnx);
            }
            else
            {
                Debug.LogWarning("Attempting to disconnect vertices that aren't connected");
            }
        }

        public override List<Connection> GetInputs()
        {
            return Inputs;
        }

        public override List<Connection> GetOutputs()
        {
            return Outputs;
        }

        /// <returns>The Output connection.</returns>
        public Connection GetOutput()
        {
            return Outputs.Find(c => c.PortName == "Output");
        }

        /// <returns>The Input connection.</returns>
        public Connection GetInput()
        {
            return Inputs.Find(c => c.PortName == "Input");
        }

        /// <summary>
        /// Determines if a port is an Output or Input. 
        /// Used when connecting and disconnecting vertices.
        /// </summary>
        /// <param name="portName"></param>
        /// <returns>True if portName is an Output port.</returns>
        protected virtual bool IsOutput(string portName)
        {
            return portName == "Output";
        }
    }
}
