// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Sprite Renderer/Blendable Color")]
    public class SpriteBlendColorVertex : TweenVertex
    {
        [SerializeField]
        private Connection _colorCnx;
        private Color _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_colorCnx != null) list.Add(_colorCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            SpriteRenderer target = GetTarget<SpriteRenderer>(DefaultTarget);
            _value = target.color;

            GetDurationDelay(out float duration, out float delay);
            Color end = _colorCnx == null ? _value : ((ValueVertex<Color>)_colorCnx.TargetCnx).Value;

            Tween tween = target
                .DOBlendableColor(end, duration)
                .SetDelay(delay)
                .SetAutoKill(false);
            SetEaseAndLoops(tween);
            SetEvents(tween);

            target.color = _value;
            return tween;
        }

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.ConnectVertex(portName, cnxPort, vertex)) return true;
            if (portName != "Color") return false;
            if (_colorCnx == null) _colorCnx = CreateInstance<Connection>();
            _colorCnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (base.DisconnectVertex(portName, cnxPort, vertex)) return true;
            if (portName != "Color") return false;
            if(_colorCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _colorCnx.DestroyCnx();
                _colorCnx = null;
                return true;
            }
            return false;
        }

        public override void Propogate(Vertex propogator, Vertex target, string port)
        {
            base.Propogate(propogator, target, port);
            if (port == "Color" && _colorCnx != null) _colorCnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            SpriteRenderer target = GetTarget<SpriteRenderer>(DefaultTarget);
            target.color = _value;
        }
    }
}
