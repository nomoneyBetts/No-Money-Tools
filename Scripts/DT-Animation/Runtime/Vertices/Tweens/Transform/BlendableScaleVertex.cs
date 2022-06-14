// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Transform/Blendable Scale")]
    public class BlendableScaleVertex : TweenVertex
    {
        [SerializeField]
        private Connection _byCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_byCnx != null) list.Add(_byCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            _value = target.localScale;

            GetDurationDelay(out float duration, out float delay);
            Vector3 by = _byCnx == null ? _value : ((ValueVertex<Vector3>)_byCnx.TargetCnx).Value;

            Tween tween = target
                .DOBlendableScaleBy(by, duration)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEaseAndLoops(tween);
            SetEvents(tween);

            target.localScale = _value;
            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            if (portName != "By") return false;
            if (_byCnx == null) _byCnx = CreateInstance<Connection>();
            _byCnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.DisconnectVertex(portName, cnxPort, vertex)) return true;
            if (portName != "By") return false;
            if (_byCnx != null && _byCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _byCnx.DestroyCnx();
                _byCnx = null;
                return true;
            }
            return false;
        }

        public override void Propogate(Vertex propogator, Vertex target, string port)
        {
            base.Propogate(propogator, target, port);
            if (port == "By" && _byCnx != null) _byCnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            target.localScale = _value;
        }
    }
}
