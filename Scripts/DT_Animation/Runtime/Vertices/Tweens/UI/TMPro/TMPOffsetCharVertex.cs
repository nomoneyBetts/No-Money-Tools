using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/TMPro/Offset Char")]
    public class TMPOffsetCharVertex : TweenVertex
    {
        [SerializeField]
        private Connection _startCnx, _endCnx, _indexCnx, _wrapperCnx;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                if (_indexCnx != null) list.Add(_indexCnx);
                if (_wrapperCnx != null) list.Add(_wrapperCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            TMP_Text target = GetTarget<TMP_Text>(DefaultTarget);

            GetDurationDelay(out float duration, out float delay);
            if (_wrapperCnx == null)
            {
                Debug.LogError("TMPro Char Tweens need a Wrapper");
                return null;
            }
            TMPWrapperVertex vw = (TMPWrapperVertex)_wrapperCnx.TargetCnx;
            if (vw.Wrapper == null) vw.SetWrapper(target);

            int index = _indexCnx == null ? 0 : ((ValueVertex<int>)_indexCnx.TargetCnx).Value;
            Vector3 start = _startCnx == null ? vw.Wrapper.GetCharOffset(index) : ((ValueVertex<Vector3>)_startCnx.TargetCnx).Value;
            Vector3 end = _endCnx == null ? vw.Wrapper.GetCharOffset(index) : ((ValueVertex<Vector3>)_endCnx.TargetCnx).Value;

            Tween tween = vw.Wrapper
                .DOOffsetChar(index, end, duration)
                .From(start)
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
                case "Start":
                    if (_startCnx == null) _startCnx = CreateInstance<Connection>();
                    cnx = _startCnx;
                    break;
                case "End":
                    if (_endCnx == null) _endCnx = CreateInstance<Connection>();
                    cnx = _endCnx;
                    break;
                case "Char Index":
                    if (_indexCnx == null) _indexCnx = CreateInstance<Connection>();
                    cnx = _indexCnx;
                    break;
                case "TMPro Wrapper":
                    if (_wrapperCnx == null) _wrapperCnx = CreateInstance<Connection>();
                    cnx = _wrapperCnx;
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
                case "Char Index":
                    if (_indexCnx != null && _indexCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _indexCnx.DestroyCnx();
                        _indexCnx = null;
                        return true;
                    }
                    break;
                case "TMPro Wrapper":
                    if (_wrapperCnx != null && _wrapperCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _wrapperCnx.DestroyCnx();
                        _wrapperCnx = null;
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
                "Char Index" => _indexCnx,
                "Start" => _startCnx,
                "End" => _endCnx,
                "TMPro Wrapper" => _wrapperCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue() { }
    }
}
