// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Audio/Set Float Mixer")]
    public class AudioMixerSetFloatVertex : TweenVertex
    {
        [SerializeField]
        private Connection _nameCnx, _startCnx, _endCnx;
        private float _value;
        private string _name;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_nameCnx != null) list.Add(_nameCnx);
                if (_startCnx != null) list.Add(_startCnx);
                if (_endCnx != null) list.Add(_endCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        { 
            AudioMixer target = GetTarget<AudioMixer>(null);
            if(target == null)
            {
                Debug.LogError("Audio Mixer Tween needs a target");
                return null;
            }

            if(_nameCnx == null)
            {
                Debug.LogError("Audio Mixer Tween needs a name");
                return null;
            }
            _name = ((ValueVertex<string>)_nameCnx.TargetCnx).Value;

            if (!target.GetFloat(_name, out _value))
            {
                Debug.LogError("Value does not exist in the mixer");
                return null;
            }

            GetDurationDelay(out float duration, out float delay);
            float start = _startCnx == null ? _value : ((ValueVertex<float>)_startCnx.TargetCnx).Value;
            float end = _endCnx == null ? _value : ((ValueVertex<float>)_endCnx.TargetCnx).Value;

            Tween tween = target
                .DOSetFloat(_name, end, duration)
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
                case "Name":
                    if (_nameCnx == null) _nameCnx = CreateInstance<Connection>();
                    cnx = _nameCnx;
                    break;
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
                case "Name":
                    if (_nameCnx != null && _nameCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _nameCnx.DestroyCnx();
                        _nameCnx = null;
                        return true;
                    }
                    break;
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
                "Name" => _nameCnx,
                "Start" => _startCnx,
                "End" => _endCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            AudioMixer target = GetTarget<AudioMixer>(null);
            target.SetFloat(_name, _value);
        }
    }
}
