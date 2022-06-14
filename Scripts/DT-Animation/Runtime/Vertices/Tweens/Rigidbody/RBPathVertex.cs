// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using DG.Tweening;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Rigidbody/Path")]
    public class RBPathVertex : TweenVertex
    {
        [SerializeField]
        private Connection _curveCnx, _modeCnx, _posConstraintCnx, _rotConstraintCnx, _lookTargetCnx,
            _lookPosCnx, _lookAheadCnx, _forwardCnx, _upCnx, _stableZCnx;
        private Vector3 _pos;
        private Quaternion _rot;

        public override Tween GenerateTween()
        {
            Rigidbody target = GetTarget<Rigidbody>(DefaultTarget);
            _pos = target.position;
            _rot = target.rotation;

            GetDurationDelay(out float duration, out float delay);
            if (_curveCnx == null)
            {
                Debug.LogError("Must input a Path Curve Getter Node");
                return null;
            }
            PathCurve path = ((ValueVertex<PathCurve>)_curveCnx.TargetCnx).Value;
            DG.Tweening.PathType type = path.PType switch
            {
                PathType.CatmullRom => DG.Tweening.PathType.CatmullRom,
                PathType.CubicBezier => DG.Tweening.PathType.CubicBezier,
                _ => DG.Tweening.PathType.Linear
            };

            PathMode pathMode = _modeCnx == null ? PathMode.Full3D : ((ValueVertex<PathMode>)_modeCnx.TargetCnx).Value;
            Transform lookAtTrans = _lookTargetCnx == null ? null : ((ValueVertex<Transform>)_lookTargetCnx.TargetCnx).Value;
            float lookAhead = _lookAheadCnx == null ? 0f : ((ValueVertex<float>)_lookAheadCnx.TargetCnx).Value;
            Vector3 lookAtPos = _lookPosCnx == null ? Vector3.zero : ((ValueVertex<Vector3>)_lookPosCnx.TargetCnx).Value;
            Vector3 forward = _forwardCnx == null ? Vector3.forward : ((ValueVertex<Vector3>)_forwardCnx.TargetCnx).Value;
            Vector3 up = _upCnx == null ? Vector3.up : ((ValueVertex<Vector3>)_upCnx.TargetCnx).Value;
            AxisConstraint posConstraint = _posConstraintCnx == null ?
                AxisConstraint.None : ((ValueVertex<AxisConstraint>)_posConstraintCnx.TargetCnx).Value;
            AxisConstraint rotConstraint = _rotConstraintCnx == null ?
                AxisConstraint.None : ((ValueVertex<AxisConstraint>)_rotConstraintCnx.TargetCnx).Value;
            bool stableZRot = _stableZCnx != null && ((ValueVertex<bool>)_stableZCnx.TargetCnx).Value;

            Tween tween = null;
            if (_lookTargetCnx != null)
            {
                if (stableZRot)
                {
                    target
                        .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                        .SetDelay(delay)
                        .SetAutoKill(false)
                        .SetOptions(path.ClosePath, posConstraint, rotConstraint)
                        .SetLookAt(lookAtTrans, stableZRot);
                }
                else
                {
                    tween = target
                        .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                        .SetDelay(delay)
                        .SetAutoKill(false)
                        .SetOptions(path.ClosePath, posConstraint, rotConstraint)
                        .SetLookAt(lookAtTrans, forward, up);
                }
            }
            else if (_lookPosCnx != null)
            {
                if (stableZRot)
                {
                    tween = target
                        .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                        .SetDelay(delay)
                        .SetAutoKill(false)
                        .SetOptions(path.ClosePath, posConstraint, rotConstraint)
                        .SetLookAt(lookAtPos, stableZRot);
                }
                else
                {
                    tween = target
                        .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                        .SetDelay(delay)
                        .SetAutoKill(false)
                        .SetOptions(path.ClosePath, posConstraint, rotConstraint)
                        .SetLookAt(lookAtPos, forward, up);
                }
            }
            else if (_lookAheadCnx != null)
            {
                if (stableZRot)
                {
                    tween = target
                        .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                        .SetDelay(delay)
                        .SetAutoKill(false)
                        .SetOptions(path.ClosePath, posConstraint, rotConstraint)
                        .SetLookAt(lookAhead, stableZRot);
                }
                else
                {
                    tween = target
                        .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                        .SetDelay(delay)
                        .SetAutoKill(false)
                        .SetOptions(path.ClosePath, posConstraint, rotConstraint)
                        .SetLookAt(lookAhead, forward, up);
                }

            }
            else
            {
                tween = target
                    .DOPath(path.DotweenWaypoints(), duration, type, pathMode, path.Resolution, path.PathColor)
                    .SetDelay(delay)
                    .SetAutoKill(false)
                    .SetOptions(path.ClosePath, posConstraint, rotConstraint);
            }
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
                case "Path Curve":
                    if (_curveCnx == null) _curveCnx = CreateInstance<Connection>();
                    cnx = _curveCnx;
                    break;
                case "Path Mode":
                    if (_modeCnx == null) _modeCnx = CreateInstance<Connection>();
                    cnx = _modeCnx;
                    break;
                case "Look-At Target":
                    if (_lookTargetCnx == null) _lookTargetCnx = CreateInstance<Connection>();
                    cnx = _lookTargetCnx;
                    break;
                case "Look-At Position":
                    if (_lookPosCnx == null) _lookPosCnx = CreateInstance<Connection>();
                    cnx = _lookPosCnx;
                    break;
                case "Look Ahead":
                    if (_lookAheadCnx == null) _lookAheadCnx = CreateInstance<Connection>();
                    cnx = _lookAheadCnx;
                    break;
                case "Pos Axis Constraint":
                    if (_posConstraintCnx == null) _posConstraintCnx = CreateInstance<Connection>();
                    cnx = _posConstraintCnx;
                    break;
                case "Rot Axis Constraint":
                    if (_rotConstraintCnx == null) _rotConstraintCnx = CreateInstance<Connection>();
                    cnx = _rotConstraintCnx;
                    break;
                case "Forward":
                    if (_forwardCnx == null) _forwardCnx = CreateInstance<Connection>();
                    cnx = _forwardCnx;
                    break;
                case "Up":
                    if (_upCnx == null) _upCnx = CreateInstance<Connection>();
                    cnx = _upCnx;
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
                case "Path Curve":
                    if (_curveCnx != null && _curveCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _curveCnx.DestroyCnx();
                        _curveCnx = null;
                        return true;
                    }
                    break;
                case "Path Mode":
                    if (_modeCnx != null && _modeCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _modeCnx.DestroyCnx();
                        _modeCnx = null;
                        return true;
                    }
                    break;
                case "Look-At Target":
                    if (_lookTargetCnx != null && _lookTargetCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _lookTargetCnx.DestroyCnx();
                        _lookTargetCnx = null;
                        return true;
                    }
                    break;
                case "Look-At Position":
                    if (_lookPosCnx != null && _lookPosCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _lookPosCnx.DestroyCnx();
                        _lookPosCnx = null;
                        return true;
                    }
                    break;
                case "Look Ahead":
                    if (_lookAheadCnx != null && _lookAheadCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _lookAheadCnx.DestroyCnx();
                        _lookAheadCnx = null;
                        return true;
                    }
                    break;
                case "Pos Axis Constraint":
                    if (_posConstraintCnx != null && _posConstraintCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _posConstraintCnx.DestroyCnx();
                        _posConstraintCnx = null;
                        return true;
                    }
                    break;
                case "Rot Axis Constraint":
                    if (_rotConstraintCnx != null && _rotConstraintCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _rotConstraintCnx.DestroyCnx();
                        _rotConstraintCnx = null;
                        return true;
                    }
                    break;
                case "Forward":
                    if (_forwardCnx != null && _forwardCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _forwardCnx.DestroyCnx();
                        _forwardCnx = null;
                        return true;
                    }
                    break;
                case "Up":
                    if (_upCnx != null && _upCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        _upCnx.DestroyCnx();
                        _upCnx = null;
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
                "Path Curve" => _curveCnx,
                "Path Mode" => _modeCnx,
                "Look-At Target" => _lookTargetCnx,
                "Look-At Position" => _lookPosCnx,
                "Look Ahead" => _lookAheadCnx,
                "Pos Axis Constraint" => _posConstraintCnx,
                "Rot Axis Constraint" => _rotConstraintCnx,
                "Forward" => _forwardCnx,
                "Up" => _upCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        public override void SetDefaultValue()
        {
            Rigidbody target = GetTarget<Rigidbody>(DefaultTarget);
            target.position = _pos;
            target.rotation = _rot;
        }
    }
}
