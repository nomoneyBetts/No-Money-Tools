// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Transform/Blendable Local Move")]
    public class BlendableLocalMoveVertex : TweenVertex
    {
        [SerializeField]
        private Connection _byCnx, _snapCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_byCnx != null) list.Add(_byCnx);
                if (_snapCnx != null) list.Add(_snapCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            _value = target.localPosition;

            GetDurationDelay(out float duration, out float delay);
            Vector3 by = _byCnx == null ? _value : ((ValueVertex<Vector3>)_byCnx.Cnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.Cnx).Value;

            Tween tween = target
                .DOBlendableLocalMoveBy(by, duration, snapping)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEaseAndLoops(tween);
            SetEvents(tween);

            target.localPosition = _value;
            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            Connection cnx;
            switch (portName)
            {
                case "By":
                    if (_byCnx == null) _byCnx = CreateInstance<Connection>();
                    cnx = _byCnx;
                    break;
                case "Snapping":
                    if (_snapCnx == null) _snapCnx = CreateInstance<Connection>();
                    cnx = _snapCnx;
                    break;
                default:
                    return false;
            }
            cnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.DisconnectVertex(portName, cnxPort, vertex)) return true;
            switch (portName)
            {
                case "By":
                    if (_byCnx != null && _byCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _byCnx.DestroyCnx();
                        _byCnx = null;
                        return true;
                    }
                    break;
                case "Snapping":
                    if (_snapCnx != null && _snapCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _snapCnx.DestroyCnx();
                        _snapCnx = null;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public override void Propogate(Vertex propogator, Vertex target, string port)
        {
            base.Propogate(propogator, target, port);
            Connection cnx = port switch
            {
                "By" => _byCnx,
                "Snapping" => _snapCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            target.localPosition = _value;
        }
    }
}
