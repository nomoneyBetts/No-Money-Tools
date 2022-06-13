using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("TMPro Wrapper")]
    public class TMPWrapperVertex : Vertex
    {
        [SerializeField]
        private Connection _outputCnx;

        public DOTweenTMPAnimator Wrapper { get; private set; }

        public override List<Connection> Inputs => new List<Connection>(0);
        public override List<Connection> Outputs
        {
            get
            {
                List<Connection> list = new List<Connection>(1);
                if (_outputCnx != null) list.Add(_outputCnx);
                return list;
            }
        }

        public void SetWrapper(TMP_Text text)
        {
            Wrapper = new DOTweenTMPAnimator(text);
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (portName != "Output") return false;
            if (_outputCnx == null) _outputCnx = CreateInstance<Connection>();
            _outputCnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (cnxPort == "Output" && _outputCnx != null && _outputCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _outputCnx.DestroyCnx();
                _outputCnx = null;
                return true;
            }
            return false;
        }
    }
}
