using UnityEngine;
using System.Collections.Generic;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Join")]
    public class JoinVertex : Vertex, IElbowPropogator
    {
        [SerializeField]
        private List<Connection> _inputs = new List<Connection>();
        [SerializeField]
        private List<Connection> _join = new List<Connection>();

        public override List<Connection> Outputs => _join;
        public override List<Connection> Inputs => _inputs;

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (_join == null) _join = new List<Connection>();
            if (portName == "Join")
            {
                Connection cnx = CreateInstance<Connection>();
                cnx.SetVals(portName, cnxPort, vertex);
                _join.Add(cnx);
                return true;
            }
            else if (portName == "Input")
            {
                Connection cnx = CreateInstance<Connection>();
                cnx.SetVals(portName, cnxPort, vertex);
                _inputs.Add(cnx);
                return true;
            }
            return false;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            List<Connection> connections;
            if (portName == "Join") connections = _join;
            else if (portName == "Input") connections = _inputs;
            else return false;

            Connection cnx = connections.Find(c => c.ValsMatch(portName, cnxPort, vertex));
            if(cnx != null)
            {
                cnx.DestroyCnx();
                connections.Remove(cnx);
                return true;
            }
            return false;
        }

        public void Propogate(Vertex propogator, Vertex target, string port)
        {
            if (port == "Input")
            {
                Connection cnx = _inputs.Find(c => c.Cnx == propogator);
                if (cnx != null) cnx.TargetCnx = target;
            }
        }
    }
}
