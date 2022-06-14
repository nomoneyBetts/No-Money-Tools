// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Rect Transform/Shape Circle")]
    public class RTransShapeCircleVertex : TweenVertex
    {
        [SerializeField]
        private Connection _centerCnx, _endCnx, _snapCnx, _relCenterCnx;
        private Vector2 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_centerCnx != null) list.Add(_centerCnx);
                if (_endCnx != null) list.Add(_endCnx);
                if (_snapCnx != null) list.Add(_snapCnx);
                if (_relCenterCnx != null) list.Add(_relCenterCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            RectTransform target = GetTarget<RectTransform>(DefaultTarget);
            _value = target.position;

            GetDurationDelay(out float duration, out float delay);
            Vector2 center = _centerCnx == null ? _value : ((ValueVertex<Vector2>)_centerCnx.TargetCnx).Value;
            float endValue = _endCnx == null ? 0f : ((ValueVertex<float>)_endCnx.TargetCnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.TargetCnx).Value;
            bool relCenter = _relCenterCnx != null && ((ValueVertex<bool>)_relCenterCnx.TargetCnx).Value;

            Tween tween = target
                .DOShapeCircle(center, endValue, duration, relCenter, snapping)
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
                case "Center":
                    if (_centerCnx == null) _centerCnx = CreateInstance<Connection>();
                    cnx = _centerCnx;
                    break;
                case "End Value Deg":
                    if (_endCnx == null) _endCnx = CreateInstance<Connection>();
                    cnx = _endCnx;
                    break;
                case "Snapping":
                    if (_snapCnx == null) _snapCnx = CreateInstance<Connection>();
                    cnx = _snapCnx;
                    break;
                case "Relative Center":
                    if (_relCenterCnx == null) _relCenterCnx = CreateInstance<Connection>();
                    cnx = _relCenterCnx;
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
                case "Center":
                    if (_centerCnx != null && _centerCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _centerCnx.DestroyCnx();
                        _centerCnx = null;
                        return true;
                    }
                    break;
                case "End Value Deg":
                    if (_endCnx != null && _endCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _endCnx.DestroyCnx();
                        _endCnx = null;
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
                case "Relative Center":
                    if (_relCenterCnx != null && _relCenterCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _relCenterCnx.DestroyCnx();
                        _relCenterCnx = null;
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
                "Center" => _centerCnx,
                "End Value Deg" => _endCnx,
                "Snapping" => _snapCnx,
                "Relative Center" => _relCenterCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            RectTransform target = GetTarget<RectTransform>(DefaultTarget);
            target.position = _value;
        }
    }
}
