using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Transform/Shake Scale")]
    public class ShakeScaleVertex : TweenVertex
    {
        [SerializeField]
        private Connection _strengthCnx, _vibratoCnx, _randomCnx, _fadeCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_strengthCnx != null) list.Add(_strengthCnx);
                if (_vibratoCnx != null) list.Add(_vibratoCnx);
                if (_randomCnx != null) list.Add(_randomCnx);
                if (_fadeCnx != null) list.Add(_fadeCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            _value = target.localScale;

            GetDurationDelay(out float duration, out float delay);
            Vector3 strength = _strengthCnx == null ? Vector3.one : ((ValueVertex<Vector3>)_strengthCnx.Cnx).Value;
            int vibrato = _vibratoCnx == null ? 10 : ((ValueVertex<int>)_vibratoCnx.Cnx).Value;
            float randomness = _randomCnx == null ? 90 : ((ValueVertex<float>)_randomCnx.Cnx).Value;
            bool fade = _fadeCnx != null && ((ValueVertex<bool>)_fadeCnx.Cnx).Value;

            Tween tween = target
                .DOShakeScale(duration, strength, vibrato, randomness, fade)
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
                case "Strength":
                    if (_strengthCnx == null) _strengthCnx = CreateInstance<Connection>();
                    cnx = _strengthCnx;
                    break;
                case "Vibrato":
                    if (_vibratoCnx == null) _vibratoCnx = CreateInstance<Connection>();
                    cnx = _vibratoCnx;
                    break;
                case "Randomness":
                    if (_randomCnx == null) _randomCnx = CreateInstance<Connection>();
                    cnx = _randomCnx;
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
                case "Strength":
                    if (_strengthCnx != null && _strengthCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _strengthCnx.DestroyCnx();
                        _strengthCnx = null;
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
                "Strength" => _strengthCnx,
                "Vibrato" => _vibratoCnx,
                "Randomness" => _randomCnx,
                "Fade Out" => _fadeCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Transform target = GetTarget<Transform>(DefaultTarget);
            target.localScale = _value;
        }
    }
}
