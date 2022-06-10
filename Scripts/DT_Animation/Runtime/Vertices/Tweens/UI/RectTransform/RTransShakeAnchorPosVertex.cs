using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Rect Transform/Shake Anchor Position")]
    public class RTransShakeAnchorPosVertex : TweenVertex
    {
        [SerializeField]
        private Connection _shakeCnx, _vibratoCnx, _randomCnx, _snapCnx, _fadeCnx;
        private Vector2 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_shakeCnx != null) list.Add(_shakeCnx);
                if (_vibratoCnx != null) list.Add(_vibratoCnx);
                if (_randomCnx != null) list.Add(_randomCnx);
                if (_snapCnx != null) list.Add(_snapCnx);
                if (_fadeCnx != null) list.Add(_fadeCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            RectTransform target = GetTarget<RectTransform>(DefaultTarget);
            _value = target.anchoredPosition;

            GetDurationDelay(out float duration, out float delay);
            Vector2 shake = _shakeCnx == null ? _value : ((ValueVertex<Vector2>)_shakeCnx.TargetCnx).Value;
            int vibrato = _vibratoCnx == null ? 10 : ((ValueVertex<int>)_vibratoCnx.TargetCnx).Value;
            float randomness = _randomCnx == null ? 1f : ((ValueVertex<float>)_randomCnx.TargetCnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.TargetCnx).Value;
            bool fadeOut = _fadeCnx != null && ((ValueVertex<bool>)_fadeCnx.TargetCnx).Value;

            Tween tween = target
                .DOShakeAnchorPos(duration, shake, vibrato, randomness, snapping, fadeOut)
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
                case "Snapping":
                    if (_snapCnx == null) _snapCnx = CreateInstance<Connection>();
                    cnx = _snapCnx;
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
                case "Snapping":
                    if (_snapCnx != null && _snapCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _snapCnx.DestroyCnx();
                        _snapCnx = null;
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
                "Shake" => _shakeCnx,
                "Vibrato" => _vibratoCnx,
                "Randomness" => _randomCnx,
                "Snapping" => _snapCnx,
                "Fade Out" => _fadeCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            RectTransform target = GetTarget<RectTransform>(DefaultTarget);
            target.anchoredPosition = _value;
        }
    }
}
