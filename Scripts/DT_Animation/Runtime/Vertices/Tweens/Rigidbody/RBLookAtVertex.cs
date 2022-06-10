using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Rigidbody/Look-At")]
    public class RBLookAtVertex : TweenVertex
    {
        [SerializeField]
        private Connection _towardsCnx, _upCnx, _axisCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_towardsCnx != null) list.Add(_towardsCnx);
                if (_upCnx != null) list.Add(_upCnx);
                if (_axisCnx != null) list.Add(_axisCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Rigidbody target = GetTarget<Rigidbody>(DefaultTarget);
            _value = target.rotation.eulerAngles;

            GetDurationDelay(out float duration, out float delay);
            Vector3 towards = _towardsCnx == null ? _value : ((ValueVertex<Vector3>)_towardsCnx.TargetCnx).Value;
            Vector3 up = _upCnx== null ? Vector3.up : ((ValueVertex<Vector3>)_upCnx.TargetCnx).Value;
            AxisConstraint axis = _axisCnx == null ? AxisConstraint.None : ((ValueVertex<AxisConstraint>)_axisCnx.TargetCnx).Value;

            Tween tween = target
                .DOLookAt(towards, duration, axis, up)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEase(tween);
            SetEvents(tween);

            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            Connection cnx;
            switch (portName)
            {
                case "Towards":
                    if (_towardsCnx == null) _towardsCnx = CreateInstance<Connection>();
                    cnx = _towardsCnx;
                    break;
                case "Up Direction":
                    if (_upCnx == null) _upCnx = CreateInstance<Connection>();
                    cnx = _upCnx;
                    break;
                case "Axis Constraint":
                    if (_axisCnx == null) _axisCnx = CreateInstance<Connection>();
                    cnx = _axisCnx;
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
                case "Towards":
                    if (_towardsCnx != null && _towardsCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _towardsCnx.DestroyCnx();
                        _towardsCnx = null;
                        return true;
                    }
                    break;
                case "Up Direction":
                    if (_upCnx != null && _upCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _upCnx.DestroyCnx();
                        _upCnx = null;
                        return true;
                    }
                    break;
                case "Axis Constraint":
                    if (_axisCnx != null && _axisCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _axisCnx.DestroyCnx();
                        _axisCnx = null;
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
                "Towards" => _towardsCnx,
                "Up Direction" => _upCnx,
                "Axis Constraint" => _axisCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Rigidbody target = GetTarget<Rigidbody>(DefaultTarget);
            target.rotation = Quaternion.Euler(_value);
        }
    }
}
