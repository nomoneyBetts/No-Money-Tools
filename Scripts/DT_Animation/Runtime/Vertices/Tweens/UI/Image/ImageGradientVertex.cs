using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Image/Gradient")]
    public class ImageGradientVertex : TweenVertex
    {
        [SerializeField]
        private Connection _endCnx;
        private Color _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_endCnx != null) list.Add(_endCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Image target = GetTarget<Image>(DefaultTarget);
            _value = target.color;
            Gradient gradient = new Gradient()
            {
                colorKeys = new GradientColorKey[2] {
                    new GradientColorKey(target.color, 0f),
                    new GradientColorKey(target.color, 1f)
                }
            };

            GetDurationDelay(out float duration, out float delay);
            Gradient end = _endCnx == null ? gradient : ((ValueVertex<Gradient>)_endCnx.TargetCnx).Value;

            Tween tween = target
                .DOGradientColor(end, duration)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEase(tween);
            SetEvents(tween);

            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            if (portName != "End") return false;
            if (_endCnx == null) _endCnx = CreateInstance<Connection>();
            _endCnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.DisconnectVertex(portName, cnxPort, vertex)) return true;
            if (_endCnx != null && _endCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _endCnx.DestroyCnx();
                _endCnx = null;
                return true;
            }
            return false;
        }

        public override void Propogate(Vertex propogator, Vertex target, string port)
        {
            base.Propogate(propogator, target, port);
            if (port == "End" && _endCnx != null) _endCnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Image target = GetTarget<Image>(DefaultTarget);
            target.color = _value;
        }
    }
}
