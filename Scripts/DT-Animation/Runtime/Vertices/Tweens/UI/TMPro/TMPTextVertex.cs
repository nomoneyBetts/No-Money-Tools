using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/UI/TMPro/Text")]
    public class TMPTextVertex : TweenVertex
    {
        [SerializeField]
        private Connection _startCnx, _endCnx, _modeCnx, _charsCnx, _richTextCnx;
        private string _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                if (_modeCnx != null) list.Add(_modeCnx);
                if (_charsCnx != null) list.Add(_charsCnx);
                if (_richTextCnx != null) list.Add(_richTextCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            TMP_Text target = GetTarget<TMP_Text>(DefaultTarget);
            _value = target.text;

            GetDurationDelay(out float duration, out float delay);
            string start = _startCnx == null ? _value : ((ValueVertex<string>)_startCnx.TargetCnx).Value;
            string end = _endCnx == null ? _value : ((ValueVertex<string>)_endCnx.TargetCnx).Value;
            string scrambleChars = _modeCnx == null ? null : ((ValueVertex<string>)_modeCnx.TargetCnx).Value;
            ScrambleMode mode = _charsCnx == null ? ScrambleMode.None : ((ValueVertex<ScrambleMode>)_charsCnx.TargetCnx).Value;
            bool richText = _richTextCnx != null && ((ValueVertex<bool>)_richTextCnx.TargetCnx).Value;

            Tween tween = target
                .DOText(end, duration, richText, mode, scrambleChars)
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
                case "Scramble Mode":
                    if (_modeCnx == null) _modeCnx = CreateInstance<Connection>();
                    cnx = _modeCnx;
                    break;
                case "Scramble Chars":
                    if (_charsCnx == null) _charsCnx = CreateInstance<Connection>();
                    cnx = _charsCnx;
                    break;
                case "Rich Text":
                    if (_richTextCnx == null) _richTextCnx = CreateInstance<Connection>();
                    cnx = _richTextCnx;
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
                case "Scramble Mode":
                    if (_modeCnx != null && _modeCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _modeCnx.DestroyCnx();
                        _modeCnx = null;
                        return true;
                    }
                    break;
                case "Scramble Chars":
                    if (_charsCnx != null && _charsCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _charsCnx.DestroyCnx();
                        _charsCnx = null;
                        return true;
                    }
                    break;
                case "Rich Text":
                    if (_richTextCnx != null && _richTextCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _richTextCnx.DestroyCnx();
                        _richTextCnx = null;
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
                "Scramble Mode" => _modeCnx,
                "Scramble Chars" => _charsCnx,
                "Rich Text" => _richTextCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            TMP_Text target = GetTarget<TMP_Text>(DefaultTarget);
            target.text = _value;
        }
    }
}
