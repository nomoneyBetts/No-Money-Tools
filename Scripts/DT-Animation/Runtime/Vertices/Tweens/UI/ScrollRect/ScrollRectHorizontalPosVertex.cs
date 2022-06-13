using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Scroll Rect/Horizontal Position")]
    public class ScrollRectHorizontalPosVertex : TweenVertex
    {
        [SerializeField]
        private Connection _endCnx, _snapCnx;
        private Vector2 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_snapCnx != null) list.Add(_snapCnx);
                if (_endCnx != null) list.Add(_endCnx);
                return list;
            }

        }

        public override Tween GenerateTween()
        {
            ScrollRect target = GetTarget<ScrollRect>(DefaultTarget);
            _value = target.normalizedPosition;
            float p = _value.x;

            GetDurationDelay(out float duration, out float delay);
            float end = _endCnx == null ? p : ((ValueVertex<float>)_endCnx.TargetCnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.TargetCnx).Value;

            Tween tween = target
                .DOHorizontalNormalizedPos(end, duration, snapping)
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
                case "Snapping":
                    if (_snapCnx == null) _snapCnx = CreateInstance<Connection>();
                    cnx = _snapCnx;
                    break;
                case "End":
                    if (_endCnx == null) _endCnx = CreateInstance<Connection>();
                    cnx = _endCnx;
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
                case "Snapping":
                    if (_snapCnx != null && _snapCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _snapCnx.DestroyCnx();
                        _snapCnx = null;
                        return true;
                    }
                    break;
                case "End":
                    if (_endCnx != null && _endCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _endCnx.DestroyCnx();
                        _endCnx = null;
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
                "Snapping" => _snapCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            ScrollRect target = GetTarget<ScrollRect>(DefaultTarget);
            target.normalizedPosition = _value;
        }
    }
}
