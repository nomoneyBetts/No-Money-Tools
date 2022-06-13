using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Elbow")]
    public class ElbowVertex : Vertex, IElbowPropogator, IDeterministicTweenChain
    {
        [SerializeField]
        private Connection _inputCnx;
        [SerializeField]
        private List<Connection> _outputs = new List<Connection>();
        [SerializeField]
        private Vertex _target;

        /// <summary>
        /// Return the first instance of another link in the chain.
        /// WARNING: Take care when using elbows on the tween chain as they are not really deterministic,
        /// and therefore not optimized for it.
        /// </summary>
        public Connection OutputCnx => _outputs.Find(cnx => cnx.Cnx is IDeterministicTweenChain || cnx.Cnx is JoinVertex);
        public override List<Connection> Outputs => _outputs;
        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = new List<Connection>(1);
                if (_inputCnx != null) list.Add(_inputCnx);
                return list;
            }
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if(portName == "Input")
            {
                if (_inputCnx == null) _inputCnx = CreateInstance<Connection>();
                _inputCnx.SetVals(portName, cnxPort, vertex);
                // If it's not an elbow, set target and begin propogation
                if (!(vertex is ElbowVertex)) Propogate(this, vertex, cnxPort);
                return true;
            }
            else if(portName == "Output")
            {
                Connection output = CreateInstance<Connection>();
                output.SetVals(portName, cnxPort, vertex);
                _outputs.Add(output);
                if (vertex is IElbowPropogator propogator) propogator.Propogate(this, _target, cnxPort);
            }
            return false;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            Connection output;
            if (_inputCnx != null && _inputCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _inputCnx.DestroyCnx();
                _inputCnx = null;
                _target = null;
                Propogate(this, null, cnxPort);
                return true;
            }
            else if((output = _outputs.Find(o => o.ValsMatch(portName, cnxPort, vertex))) != null)
            {
                output.DestroyCnx();
                _outputs.Remove(output);
                return true;
            }
            return false;
        }

        public void Propogate(Vertex propogator, Vertex target, string port)
        {
            _target = target;
            foreach (Connection output in _outputs)
            {
                if (output is IElbowPropogator prop) prop.Propogate(this, _target, output.CnxPort);
            }
        }
    }
}
