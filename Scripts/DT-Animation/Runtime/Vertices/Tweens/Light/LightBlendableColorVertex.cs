// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Light/Blendable Color")]
    public class LightBlendableColorVertex : TweenVertex
    {
        [SerializeField]
        private Connection _endCnx;
        private Color _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_endCnx != null) list.Add(_endCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Light target = GetTarget<Light>(DefaultTarget);
            _value = target.color;

            GetDurationDelay(out float duration, out float delay);
            Color end = _endCnx == null ? _value : ((ValueVertex<Color>)_endCnx.TargetCnx).Value;

            Tween tween = target
                .DOBlendableColor(end, duration)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEaseAndLoops(tween);
            SetEvents(tween);

            target.color = _value;
            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            if (portName != "End") return false;
            if (_endCnx == null) _endCnx = CreateInstance<Connection>();
            _endCnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.DisconnectVertex(portName, cnxPort, vertex)) return true;
            if(_endCnx != null && _endCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _endCnx.DestroyCnx();
                _endCnx = null;
                return true;
            }
            return false;
        }

        public override void Propogate(Vertex propogator, Vertex target, string port)
        {
            base.Propogate(propogator, target, port);
            if (port == "End" && _endCnx != null) _endCnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Light target = GetTarget<Light>(DefaultTarget);
            target.color = _value;
        }
    }
}
