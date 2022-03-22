using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public abstract class ValueVertex<T> : Vertex 
    {
        public List<Connection> Outputs = new List<Connection>();

        public T Value;

        public override void ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (portName == "Output")
            {
                Connection cnx = CreateInstance<Connection>();
                cnx.SetVals(portName, cnxPort, vertex);
                Outputs.Add(cnx);
            }
            else
            {
                Debug.LogWarning("Connecting to unknown port name on Value Node: " + portName);
            }
        }

        public override void DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            Connection cnx = Outputs.Find(c => c.ValsMatch(portName, cnxPort, vertex));
            if (cnx != null)
            {
                cnx.DestroyCnx();
                Outputs.Remove(cnx);
            }
            else
            {
                Debug.LogWarning("Attempting to disconnect vertices that aren't connected");
            }
        }

        public override List<Connection> GetInputs()
        {
            throw new System.NotImplementedException();
        }

        public override List<Connection> GetOutputs()
        {
            return Outputs;
        }
    }
}
