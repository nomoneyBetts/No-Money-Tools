// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public abstract class EventVertex : Vertex
    {
        public ExposedEvent ExposedEvent;
        [SerializeField]
        private List<Connection> _outputs = new List<Connection>();

        public override List<Connection> Inputs => new List<Connection>(0);
        public override List<Connection> Outputs => _outputs;

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if(portName == "Output")
            {
                Connection output = CreateInstance<Connection>();
                output.SetVals(portName, cnxPort, vertex);
                _outputs.Add(output);
                return true;
            }
            return false;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            Connection output;
            if((output = _outputs.Find(cnx => cnx.ValsMatch(portName, cnxPort, vertex))) != null)
            {
                output.DestroyCnx();
                _outputs.Remove(output);
                return true;
            }
            return false;
        }
    }
}
