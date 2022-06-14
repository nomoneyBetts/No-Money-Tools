using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Misc/Line Color")]
    public class LineColorVertex : TweenVertex
    {
        [SerializeField]
        private Connection _aStart, _aEnd, _bStart, _bEnd;
        private Color _start, _end;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_aStart != null) list.Add(_aStart);
                if (_aEnd != null) list.Add(_aEnd);
                if (_bStart != null) list.Add(_bStart);
                if (_bEnd != null) list.Add(_bEnd);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            LineRenderer target = GetTarget<LineRenderer>(DefaultTarget);
            _start = target.startColor;
            _end = target.endColor;
            Color2 start = new Color2();
            Color2 end = new Color2();

            GetDurationDelay(out float duration, out float delay);
            start.ca = _aStart == null ? _start : ((ValueVertex<Color>)_aStart.TargetCnx).Value;
            start.cb = _aEnd == null ? _end : ((ValueVertex<Color>)_aEnd.TargetCnx).Value;
            end.ca = _bStart == null ? _start : ((ValueVertex<Color>)_bStart.TargetCnx).Value;
            end.cb = _bEnd == null ? _end : ((ValueVertex<Color>)_bEnd.TargetCnx).Value;

            Tween tween = target
                .DOColor(start, end, duration)
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
                case "A) Start Color":
                    if (_aStart == null) _aStart= CreateInstance<Connection>();
                    cnx = _aStart;
                    break;
                case "A) End Color":
                    if (_aEnd == null) _aEnd = CreateInstance<Connection>();
                    cnx = _aEnd;
                    break;
                case "B) Start Color":
                    if (_bStart == null) _bStart = CreateInstance<Connection>();
                    cnx = _bStart;
                    break;
                case "B) End Color":
                    if (_bEnd == null) _bEnd = CreateInstance<Connection>();
                    cnx = _bEnd;
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
                case "A) Start Color":
                    if (_aStart != null && _aStart.ValsMatch(portName, cnxPort, vertex))
                    {
                        _aStart.DestroyCnx();
                        _aStart = null;
                        return true;
                    }
                    break;
                case "A) End Color":
                    if (_aEnd != null && _aEnd.ValsMatch(portName, cnxPort, vertex))
                    {
                        _aEnd.DestroyCnx();
                        _aEnd = null;
                        return true;
                    }
                    break;
                case "B) Start Color":
                    if (_bStart != null && _bStart.ValsMatch(portName, cnxPort, vertex))
                    {
                        _bStart.DestroyCnx();
                        _bStart = null;
                        return true;
                    }
                    break;
                case "B) End Color":
                    if (_bEnd != null && _bEnd.ValsMatch(portName, cnxPort, vertex))
                    {
                        _bEnd.DestroyCnx();
                        _bEnd = null;
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
                "A) Start Color" => _aStart,
                "A) End Color" => _aEnd,
                "B) Start Color" => _bStart,
                "B) End Color" => _bEnd,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            LineRenderer target = GetTarget<LineRenderer>(DefaultTarget);
            target.startColor = _start;
            target.endColor = _end;
        }
    }
}
