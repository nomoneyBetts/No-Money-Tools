// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/TMPro/Glow Color")]
    public class TMPGlowColorVertex : TweenVertex
    {
        [SerializeField]
        private Connection _endCnx, _sharedCnx;
        private Color _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_sharedCnx != null) list.Add(_sharedCnx);
                if (_endCnx != null) list.Add(_endCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            TMP_Text target = GetTarget<TMP_Text>(DefaultTarget);
            _value = target.color;

            GetDurationDelay(out float duration, out float delay);
            Color end = _endCnx == null ? target.color : ((ValueVertex<Color>)_endCnx.TargetCnx).Value;
            bool sharedMat = _sharedCnx != null && ((ValueVertex<bool>)_sharedCnx.TargetCnx).Value;

            Tween tween = target
                .DOGlowColor(end, duration, sharedMat)
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
                case "Shared Material":
                    if (_sharedCnx == null) _sharedCnx = CreateInstance<Connection>();
                    cnx = _sharedCnx;
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
                case "Shared Material":
                    if (_sharedCnx != null && _sharedCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _sharedCnx.DestroyCnx();
                        _sharedCnx = null;
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
                "End" => _endCnx,
                "Shared Material" => _sharedCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            TMP_Text target = GetTarget<TMP_Text>(DefaultTarget);
            target.color = _value;
        }
    }
}
