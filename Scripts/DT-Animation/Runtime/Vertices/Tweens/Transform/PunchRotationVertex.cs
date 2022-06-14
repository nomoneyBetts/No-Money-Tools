// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Transform/Punch Rotation")]
    public class PunchRotationVertex : TweenVertex
    {
        [SerializeField]
        private Connection _punchCnx, _vibratoCnx, _elasticityCnx;
        private Quaternion _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_punchCnx != null) list.Add(_punchCnx);
                if (_vibratoCnx != null) list.Add(_vibratoCnx);
                if (_elasticityCnx != null) list.Add(_elasticityCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            _value = target.rotation;

            GetDurationDelay(out float duration, out float delay);
            Vector3 punch = _punchCnx == null ? Vector3.zero : ((ValueVertex<Vector3>)_punchCnx.TargetCnx).Value;
            int vibrato = _vibratoCnx == null ? 10 : ((ValueVertex<int>)_vibratoCnx.TargetCnx).Value;
            float elasticity = _elasticityCnx == null ? 1f : ((ValueVertex<float>)_elasticityCnx.TargetCnx).Value;

            Tween tween = target
                .DOPunchRotation(punch, duration, vibrato, elasticity)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEaseAndLoops(tween);
            SetEvents(tween);

            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            Connection cnx;
            switch (portName)
            {
                case "Punch":
                    if (_punchCnx == null) _punchCnx = CreateInstance<Connection>();
                    cnx = _punchCnx;
                    break;
                case "Vibrato":
                    if (_vibratoCnx == null) _vibratoCnx = CreateInstance<Connection>();
                    cnx = _vibratoCnx;
                    break;
                case "Elasticity":
                    if (_elasticityCnx == null) _elasticityCnx = CreateInstance<Connection>();
                    cnx = _elasticityCnx;
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
                case "Punch":
                    if (_punchCnx != null && _punchCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _punchCnx.DestroyCnx();
                        _punchCnx = null;
                        return true;
                    }
                    break;
                case "Vibrato":
                    if (_vibratoCnx != null && _vibratoCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _vibratoCnx.DestroyCnx();
                        _vibratoCnx = null;
                        return true;
                    }
                    break;
                case "Elasticity":
                    if (_elasticityCnx != null && _elasticityCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _elasticityCnx.DestroyCnx();
                        _elasticityCnx = null;
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
                "Punch" => _punchCnx,
                "Vibrato" => _vibratoCnx,
                "Elasticity" => _elasticityCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            target.rotation = _value;
        }
    }
}
