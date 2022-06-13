using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/TMPro/Punch Char Scale")]
    public class TMPPunchCharScaleVertex : TweenVertex
    {
        [SerializeField]
        private Connection _indexCnx, _punchCnx, _vibratoCnx, _elasticityCnx, _wrapperCnx;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_indexCnx != null) list.Add(_indexCnx);
                if (_punchCnx != null) list.Add(_punchCnx);
                if (_vibratoCnx != null) list.Add(_vibratoCnx);
                if (_elasticityCnx != null) list.Add(_elasticityCnx);
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
            Vector3 punch = _punchCnx == null ? Vector3.zero : ((ValueVertex<Vector3>)_punchCnx.TargetCnx).Value;
            int vibrato = _vibratoCnx == null ? 10 : ((ValueVertex<int>)_vibratoCnx.TargetCnx).Value;
            float elasticity = _elasticityCnx == null ? 1f : ((ValueVertex<float>)_elasticityCnx.TargetCnx).Value;

            Tween tween = vw.Wrapper
                .DOPunchCharScale(index, punch, duration, vibrato, elasticity)
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
                case "Char Index":
                    if (_indexCnx == null) _indexCnx = CreateInstance<Connection>();
                    cnx = _indexCnx;
                    break;
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
                case "Char Index":
                    if (_indexCnx != null && _indexCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _indexCnx.DestroyCnx();
                        _indexCnx = null;
                        return true;
                    }
                    break;
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
                "Punch" => _punchCnx,
                "Vibrato" => _vibratoCnx,
                "Elasticity" => _elasticityCnx,
                "TMPro Wrapper" => _wrapperCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue() { }
    }
}
