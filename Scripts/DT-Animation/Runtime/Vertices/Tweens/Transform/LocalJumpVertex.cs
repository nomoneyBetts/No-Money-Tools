using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Transform/Local Jump")]
    public class LocalJumpVertex : TweenVertex
    {
        [SerializeField]
        private Connection _endCnx, _powerCnx, _numCnx, _snapCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_endCnx != null) list.Add(_endCnx);
                if (_powerCnx != null) list.Add(_powerCnx);
                if (_numCnx != null) list.Add(_numCnx);
                if (_snapCnx != null) list.Add(_snapCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            _value = target.localPosition;

            GetDurationDelay(out float duration, out float delay);
            Vector3 end = _endCnx == null ? _value : ((ValueVertex<Vector3>)_endCnx.TargetCnx).Value;
            float jumpPwr = _powerCnx == null ? 0f : ((ValueVertex<float>)_powerCnx.TargetCnx).Value;
            int numJumps = _numCnx == null ? 1 : ((ValueVertex<int>)_numCnx.TargetCnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.TargetCnx).Value;

            Tween tween = target
                .DOLocalJump(end, jumpPwr, numJumps, duration, snapping)
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
                case "End":
                    if (_endCnx == null) _endCnx = CreateInstance<Connection>();
                    cnx = _endCnx;
                    break;
                case "Jump Power":
                    if (_powerCnx == null) _powerCnx = CreateInstance<Connection>();
                    cnx = _powerCnx;
                    break;
                case "Num Jumps":
                    if (_numCnx == null) _numCnx = CreateInstance<Connection>();
                    cnx = _numCnx;
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
                case "End":
                    if (_endCnx != null && _endCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _endCnx.DestroyCnx();
                        _endCnx = null;
                        return true;
                    }
                    break;
                case "Jump Power":
                    if (_powerCnx != null && _powerCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _powerCnx.DestroyCnx();
                        _powerCnx = null;
                        return true;
                    }
                    break;
                case "Num Jumps":
                    if (_numCnx != null && _numCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _numCnx.DestroyCnx();
                        _numCnx = null;
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
                "End" => _endCnx,
                "Jump Power" => _powerCnx,
                "Num Jumps" => _numCnx,
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
