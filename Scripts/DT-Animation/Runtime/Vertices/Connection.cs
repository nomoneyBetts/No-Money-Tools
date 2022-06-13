using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class Connection : ScriptableObject
    {
        public string PortName, CnxPort;
        public Vertex Cnx, TargetCnx;

        public void SetVals(string portName, string cnxPort, Vertex cnxVertex)
        {
            PortName = portName;
            CnxPort = cnxPort;
            Cnx = cnxVertex;
            TargetCnx = cnxVertex;
        }

        public bool ValsMatch(string portName, string cnxPort, Vertex cnxVertex)
        {
            return portName == PortName && CnxPort == cnxPort && cnxVertex == Cnx;
        }

        public void DestroyCnx()
        {
            DestroyImmediate(this);
        }
    }
}
