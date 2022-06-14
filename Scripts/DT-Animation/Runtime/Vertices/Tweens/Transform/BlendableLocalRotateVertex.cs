using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Transform/Blendable Local Rotate")]
    public class BlendableLocalRotateVertex : TweenVertex
    {
        [SerializeField]
        private Connection _byCnx, _modeCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_byCnx != null) list.Add(_byCnx);
                if (_modeCnx != null) list.Add(_modeCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            _value = target.localRotation.eulerAngles;

            GetDurationDelay(out float duration, out float delay);
            Vector3 by = _byCnx == null ? _value : ((ValueVertex<Vector3>)_byCnx.TargetCnx).Value;
            RotateMode mode = _modeCnx == null ? RotateMode.Fast : ((ValueVertex<RotateMode>)_modeCnx.TargetCnx).Value;

            Tween tween = target
                .DOBlendableLocalRotateBy(by, duration, mode)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEaseAndLoops(tween);
            SetEvents(tween);

            target.localRotation = Quaternion.Euler(_value);
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
                case "Rotate Mode":
                    if (_modeCnx == null) _modeCnx = CreateInstance<Connection>();
                    cnx = _modeCnx;
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
                case "Rotate Mode":
                    if (_modeCnx != null && _modeCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _modeCnx.DestroyCnx();
                        _modeCnx = null;
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
                "Rotate Mode" => _modeCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            target.localRotation = Quaternion.Euler(_value);
        }
    }
}
