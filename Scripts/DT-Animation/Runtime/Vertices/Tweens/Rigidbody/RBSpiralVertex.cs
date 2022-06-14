// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Rigidbody/Spiral")]
    public class RBSpiralVertex : TweenVertex
    {
        [SerializeField]
        private Connection _axisCnx, _modeCnx, _speedCnx, _freqCnx, _depthCnx, _snapCnx;
        private Vector3 _value;

        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = base.Inputs;
                if (_axisCnx != null) list.Add(_axisCnx);
                if (_modeCnx != null) list.Add(_modeCnx);
                if (_speedCnx != null) list.Add(_speedCnx);
                if (_freqCnx != null) list.Add(_freqCnx);
                if (_depthCnx != null) list.Add(_depthCnx);
                if (_snapCnx != null) list.Add(_snapCnx);
                return list;
            }
        }

        public override Tween GenerateTween()
        {
            Rigidbody target = GetTarget<Rigidbody>(DefaultTarget);
            _value = target.position;

            GetDurationDelay(out float duration, out float delay);
            Vector3 axis = _axisCnx == null ? Vector3.up : ((ValueVertex<Vector3>)_axisCnx.TargetCnx).Value;
            SpiralMode mode = _modeCnx == null ? SpiralMode.Expand : ((ValueVertex<SpiralMode>)_modeCnx.TargetCnx).Value;
            float speed = _speedCnx == null ? 1f : ((ValueVertex<float>)_speedCnx.TargetCnx).Value;
            float frequency = _freqCnx == null ? 10f : ((ValueVertex<float>)_freqCnx.TargetCnx).Value;
            float depth = _depthCnx == null ? 0f : ((ValueVertex<float>)_depthCnx.TargetCnx).Value;
            bool snapping = _snapCnx != null && ((ValueVertex<bool>)_snapCnx.TargetCnx).Value;

            Tween tween = target
                .DOSpiral(duration, axis, mode, speed, frequency, depth, snapping)
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
                case "Axis":
                    if (_axisCnx == null) _axisCnx = CreateInstance<Connection>();
                    cnx = _axisCnx;
                    break;
                case "Spiral Mode":
                    if (_modeCnx == null) _modeCnx = CreateInstance<Connection>();
                    cnx = _modeCnx;
                    break;
                case "Speed":
                    if (_speedCnx == null) _speedCnx = CreateInstance<Connection>();
                    cnx = _speedCnx;
                    break;
                case "Frequency":
                    if (_freqCnx == null) _freqCnx = CreateInstance<Connection>();
                    cnx = _freqCnx;
                    break;
                case "Depth":
                    if (_depthCnx == null) _depthCnx = CreateInstance<Connection>();
                    cnx = _depthCnx;
                    break;
                case "Snapping":
                    if (_snapCnx == null) _snapCnx = CreateInstance<Connection>();
                    cnx = _snapCnx;
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
                case "Axis":
                    if (_axisCnx != null && _axisCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _axisCnx.DestroyCnx();
                        _axisCnx = null;
                        return true;
                    }
                    break;
                case "Spiral Mode":
                    if (_modeCnx != null && _modeCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _modeCnx.DestroyCnx();
                        _modeCnx = null;
                        return true;
                    }
                    break;
                case "Speed":
                    if (_speedCnx != null && _speedCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _speedCnx.DestroyCnx();
                        _speedCnx = null;
                        return true;
                    }
                    break;
                case "Frequency":
                    if (_freqCnx != null && _freqCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _freqCnx.DestroyCnx();
                        _freqCnx = null;
                        return true;
                    }
                    break;
                case "Depth":
                    if (_depthCnx != null && _depthCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _depthCnx.DestroyCnx();
                        _depthCnx = null;
                        return true;
                    }
                    break;
                case "Snapping":
                    if (_snapCnx != null && _snapCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _snapCnx.DestroyCnx();
                        _snapCnx = null;
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
                "Axis" => _axisCnx,
                "Spiral Mode" => _modeCnx,
                "Speed" => _speedCnx,
                "Frequency" => _freqCnx,
                "Depth" => _depthCnx,
                "Snapping" => _snapCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Rigidbody target = GetTarget<Rigidbody>(DefaultTarget);
            target.position = _value;
        }
    }
}
