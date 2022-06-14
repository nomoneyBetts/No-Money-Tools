using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Globalization;
using System.Collections.Generic;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/TMPro/Counter")]
    public class TMPCounterVertex : TweenVertex
    {
        [SerializeField]
        private Connection _startCnx, _endCnx, _infoCnx, _sepCnx;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                if (_infoCnx != null) list.Add(_infoCnx);
                if (_sepCnx != null) list.Add(_sepCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            TMP_Text target = GetTarget<TMP_Text>(DefaultTarget);

            GetDurationDelay(out float duration, out float delay);
            int start = _startCnx == null ? 0 : ((ValueVertex<int>)_startCnx.TargetCnx).Value;
            int end = _endCnx == null ? 0 : ((ValueVertex<int>)_endCnx.TargetCnx).Value;
            CultureInfo info = _infoCnx == null ? null : ((ValueVertex<CultureInfo>)_infoCnx.TargetCnx).Value;
            bool separator = _sepCnx != null && ((ValueVertex<bool>)_sepCnx.TargetCnx).Value;

            Tween tween = target
                .DOCounter(start, end, duration, separator, info)
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
                case "Thousands Separator":
                    if (_sepCnx == null) _sepCnx = CreateInstance<Connection>();
                    cnx = _sepCnx;
                    break;
                case "Culture Info":
                    if (_infoCnx == null) _infoCnx = CreateInstance<Connection>();
                    cnx = _infoCnx;
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
                case "Thousands Separator":
                    if (_sepCnx != null && _sepCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _sepCnx.DestroyCnx();
                        _sepCnx = null;
                        return true;
                    }
                    break;
                case "Culture Info":
                    if (_infoCnx != null && _infoCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _infoCnx.DestroyCnx();
                        _infoCnx = null;
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
                "Thousands Separator" => _sepCnx,
                "Culture Info" => _infoCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue() { }
    }
}
