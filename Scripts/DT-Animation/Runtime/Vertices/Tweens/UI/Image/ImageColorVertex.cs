using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Image/Color")]
    public class ImageColorVertex : TweenVertex
    {
        [SerializeField]
        private Connection _startCnx, _endCnx;
        private Color _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Image target = GetTarget<Image>(DefaultTarget);
            _value = target.color;

            GetDurationDelay(out float duration, out float delay);
            Color start = _startCnx == null ? _value : ((ValueVertex<Color>)_startCnx.TargetCnx).Value;
            Color end = _endCnx == null ? _value : ((ValueVertex<Color>)_endCnx.TargetCnx).Value;

            Tween tween = target
                .DOColor(end, duration)
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
                    cnx = _startCnx;
                    break;
                case "End":
                    cnx = _endCnx;
                    break;
                default:
                    return false;
            }
            if (cnx == null) cnx = CreateInstance<Connection>();
            cnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.DisconnectVertex(portName, cnxPort, vertex)) return true;
            switch (portName)
            {
                case "Start":
                    if (_startCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _startCnx.DestroyCnx();
                        _startCnx = null;
                        return true;
                    }
                    break;
                case "End":
                    if (_endCnx.ValsMatch(portName, cnxPort, vertex))
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
                "Start" => _startCnx,
                "End" => _endCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Image target = GetTarget<Image>(DefaultTarget);
            target.color = _value;
        }
    }
}
