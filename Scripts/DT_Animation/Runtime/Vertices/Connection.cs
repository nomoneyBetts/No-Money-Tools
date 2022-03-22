using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class Connection : ScriptableObject
    {
        public string PortName, CnxPort;
        public Vertex CnxVertex;

        public void SetVals(string portName, string cnxPort, Vertex cnxVertex)
        {
            PortName = portName;
            CnxPort = cnxPort;
            CnxVertex = cnxVertex;
        }

        public bool ValsMatch(string portName, string cnxPort, Vertex cnxVertex)
        {
            return portName == PortName && CnxPort == cnxPort && cnxVertex == CnxVertex;
        }

        public void DestroyCnx()
        {
            DestroyImmediate(this);
        }
    }
}
