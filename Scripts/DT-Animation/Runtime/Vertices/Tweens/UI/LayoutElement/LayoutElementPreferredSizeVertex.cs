using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Layout Element/Preferred Size")]
    public class LayoutElementPreferredSizeVertex : TweenVertex
    {
        [SerializeField]
        private Connection _startCnx, _endCnx, _snapCnx;
        private Vector2 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                if (_snapCnx != null) list.Add(_snapCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            LayoutElement target = GetTarget<LayoutElement>(DefaultTarget);
            _value = new Vector2(target.preferredWidth, target.preferredHeight);

            GetDurationDelay(out float duration, out float delay);
            Vector2 start = _startCnx == null ? _value : ((ValueVertex<Vector2>)_startCnx.TargetCnx).Value;
            Vector2 end = _endCnx == null ? _value : ((ValueVertex<Vector2>)_endCnx.TargetCnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.TargetCnx).Value;

            Tween tween = target
                .DOPreferredSize(end, duration, snapping)
                .From(start)
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
                case "Start":
                    if (_startCnx == null) _startCnx = CreateInstance<Connection>();
                    cnx = _startCnx;
                    break;
                case "End":
                    if (_endCnx == null) _endCnx = CreateInstance<Connection>();
                    cnx = _endCnx;
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
                case "Start":
                    if (_startCnx != null && _startCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _startCnx.DestroyCnx();
                        _startCnx = null;
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
                "Start" => _startCnx,
                "End" => _endCnx,
                "Snapping" => _snapCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            LayoutElement target = GetTarget<LayoutElement>(DefaultTarget);
            target.preferredWidth = _value.x;
            target.preferredHeight = _value.y;
        }
    }
}
