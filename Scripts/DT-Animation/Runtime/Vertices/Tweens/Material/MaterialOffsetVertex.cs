using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Material/Offset")]
    public class MaterialOffsetVertex : TweenVertex
    {
        [SerializeField]
        private Connection _startCnx, _endCnx, _matIndexCnx, _propNameCnx, _propIdCnx;
        private Material[] _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                if (_matIndexCnx != null) list.Add(_matIndexCnx);
                if (_propNameCnx != null) list.Add(_propNameCnx);
                if (_propIdCnx != null) list.Add(_propIdCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            MeshRenderer target = GetTarget<MeshRenderer>(DefaultTarget);
            _value = target.materials;

            GetDurationDelay(out float duration, out float delay);
            int index = _matIndexCnx == null ? 0 : ((ValueVertex<int>)_matIndexCnx.TargetCnx).Value;
            string propstr = _propNameCnx== null ? null : ((ValueVertex<string>)_propNameCnx.TargetCnx).Value;

            Tween tween;
            if(propstr == null)
            {
                if(_propIdCnx == null)
                {
                    Vector2 offset = _value[index].mainTextureOffset;
                    Vector2 start = _startCnx == null ? offset : ((ValueVertex<Vector2>)_startCnx.TargetCnx).Value;
                    Vector2 end = _endCnx== null ? offset : ((ValueVertex<Vector2>)_endCnx.TargetCnx).Value;
                    tween = _value[index]
                        .DOOffset(end, duration)
                        .From(start);
                }
                else
                {
                    int propid = ((ValueVertex<int>)_propIdCnx.TargetCnx).Value;
                    Vector2 offset = _value[index].GetTextureOffset(propid);
                    Vector2 start = _startCnx == null ? offset : ((ValueVertex<Vector2>)_startCnx.TargetCnx).Value;
                    Vector2 end = _endCnx == null ? offset : ((ValueVertex<Vector2>)_endCnx.TargetCnx).Value;
                    tween = _value[index]
                        .DOOffset(end, propid, duration)
                        .From(start);
                }
            }
            else
            {
                Vector2 offset = _value[index].GetTextureOffset(propstr);
                Vector2 start = _startCnx == null ? offset : ((ValueVertex<Vector2>)_startCnx.TargetCnx).Value;
                Vector2 end = _endCnx == null ? offset : ((ValueVertex<Vector2>)_endCnx.TargetCnx).Value;
                tween = _value[index]
                    .DOOffset(end, propstr, duration)
                    .From(start);
            }

            tween
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
                case "Material Index":
                    if (_matIndexCnx == null) _matIndexCnx = CreateInstance<Connection>();
                    cnx = _matIndexCnx;
                    break;
                case "Property Name":
                    if (_propNameCnx == null) _propNameCnx = CreateInstance<Connection>();
                    cnx = _propNameCnx;
                    break;
                case "Property ID":
                    if (_propIdCnx == null) _propIdCnx = CreateInstance<Connection>();
                    cnx = _propIdCnx;
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
                case "Property ID":
                    if (_propIdCnx != null && _propIdCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _propIdCnx.DestroyCnx();
                        _propIdCnx = null;
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
                "Material Index" => _matIndexCnx,
                "Property Name" => _propNameCnx,
                "Property ID" => _propIdCnx,
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
