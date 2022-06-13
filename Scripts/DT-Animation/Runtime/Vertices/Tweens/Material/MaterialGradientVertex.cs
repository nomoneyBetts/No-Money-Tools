using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Material/Gradient")]
    public class MaterialGradientVertex : TweenVertex
    {
        [SerializeField]
        private Connection _endCnx, _matIndexCnx, _propNameCnx;
        private Material[] _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_endCnx != null) list.Add(_endCnx);
                if (_matIndexCnx != null) list.Add(_matIndexCnx);
                if (_propNameCnx != null) list.Add(_propNameCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            MeshRenderer target = GetTarget<MeshRenderer>(DefaultTarget);
            _value = target.materials;

            GetDurationDelay(out float duration, out float delay);
            int index = _matIndexCnx == null ? 0 : ((ValueVertex<int>)_matIndexCnx.TargetCnx).Value;
            string propstr = _propNameCnx == null ? null : ((ValueVertex<string>)_propNameCnx.TargetCnx).Value;
            Gradient end = _endCnx == null ? new Gradient() : ((ValueVertex<Gradient>)_endCnx.TargetCnx).Value;

            Tween tween = propstr == null ?
                _value[index]
                    .DOGradientColor(end, duration)
                :
                _value[index]
                    .DOGradientColor(end, propstr, duration);

            tween
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
                case "End":
                    if (_endCnx == null) _endCnx = CreateInstance<Connection>();
                    cnx = _endCnx;
                    break;
                case "Material Index":
                    if (_matIndexCnx == null) _matIndexCnx = CreateInstance<Connection>();
                    cnx = _matIndexCnx;
                    break;
                case "Property Name":
                    if (_propNameCnx == null) _propNameCnx = CreateInstance<Connection>();
                    cnx = _propNameCnx;
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
                case "End":
                    if (_endCnx != null && _endCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _endCnx.DestroyCnx();
                        _endCnx = null;
                        return true;
                    }
                    break;
                case "Material Index":
                    if (_matIndexCnx != null && _matIndexCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _matIndexCnx.DestroyCnx();
                        _matIndexCnx = null;
                        return true;
                    }
                    break;
                case "Property Name":
                    if (_propNameCnx != null && _propNameCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _propNameCnx.DestroyCnx();
                        _propNameCnx = null;
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
                "Material Index" => _matIndexCnx,
                "Property Name" => _propNameCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            MeshRenderer target = GetTarget<MeshRenderer>(DefaultTarget);
            target.materials = _value;
        }
    }
}
