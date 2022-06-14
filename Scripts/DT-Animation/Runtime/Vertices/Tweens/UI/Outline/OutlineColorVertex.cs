// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/Outline/Color")]
    public class OutlineColorVertex : TweenVertex
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
            Outline target = GetTarget<Outline>(DefaultTarget);
            _value = target.effectColor;

            GetDurationDelay(out float duration, out float delay);
            Color start = _startCnx == null ? _value : ((ValueVertex<Color>)_startCnx.TargetCnx).Value;
            Color end = _endCnx == null ? _value : ((ValueVertex<Color>)_endCnx.TargetCnx).Value;

            Tween tween = target
                .DOColor(end, duration)
                .From(start)
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
            Outline target = GetTarget<Outline>(DefaultTarget);
            target.effectColor = _value;
        }
    }
}
