using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/TMPro/Shake Char Scale")]
    public class TMPShakeCharScaleVertex : TweenVertex
    {
        [SerializeField]
        private Connection _indexCnx, _shakeCnx, _vibratoCnx, _randomCnx, _wrapperCnx, _fadeCnx;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_indexCnx != null) list.Add(_indexCnx);
                if (_shakeCnx != null) list.Add(_shakeCnx);
                if (_vibratoCnx != null) list.Add(_vibratoCnx);
                if (_randomCnx != null) list.Add(_randomCnx);
                if (_fadeCnx != null) list.Add(_fadeCnx);
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
            Vector3 shake = _shakeCnx == null ? Vector3.one : ((ValueVertex<Vector3>)_shakeCnx.TargetCnx).Value;
            int vibrato = _vibratoCnx == null ? 10 : ((ValueVertex<int>)_vibratoCnx.TargetCnx).Value;
            float randomness = _randomCnx == null ? 1f : ((ValueVertex<float>)_randomCnx.TargetCnx).Value;
            bool fadeOut = _fadeCnx != null && ((ValueVertex<bool>)_fadeCnx.TargetCnx).Value;

            Tween tween = vw.Wrapper
                .DOShakeCharOffset(index, duration, shake, vibrato, randomness, fadeOut)
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
                case "Shake":
                    if (_shakeCnx == null) _shakeCnx = CreateInstance<Connection>();
                    cnx = _shakeCnx;
                    break;
                case "Vibrato":
                    if (_vibratoCnx == null) _vibratoCnx = CreateInstance<Connection>();
                    cnx = _vibratoCnx;
                    break;
                case "Randomness":
                    if (_randomCnx == null) _randomCnx = CreateInstance<Connection>();
                    cnx = _randomCnx;
                    break;
                case "TMPro Wrapper":
                    if (_wrapperCnx == null) _wrapperCnx = CreateInstance<Connection>();
                    cnx = _wrapperCnx;
                    break;
                case "Fade Out":
                    if (_fadeCnx == null) _fadeCnx = CreateInstance<Connection>();
                    cnx = _fadeCnx;
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
                case "Shake":
                    if (_shakeCnx != null && _shakeCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _shakeCnx.DestroyCnx();
                        _shakeCnx = null;
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
                case "Randomness":
                    if (_randomCnx != null && _randomCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _randomCnx.DestroyCnx();
                        _randomCnx = null;
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
                case "Fade Out":
                    if (_fadeCnx != null && _fadeCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _fadeCnx.DestroyCnx();
                        _fadeCnx = null;
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
                "Shake" => _shakeCnx,
                "Vibrato" => _vibratoCnx,
                "Randomness" => _randomCnx,
                "TMPro Wrapper" => _wrapperCnx,
                "Fade Out" => _fadeCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue() { }
    }
}
