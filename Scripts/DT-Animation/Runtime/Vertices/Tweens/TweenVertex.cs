using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NoMoney.DTAnimation
{
    public abstract class TweenVertex : Vertex, IElbowPropogator, IDeterministicTweenChain
    {
        public Object DefaultTarget;

        [SerializeField]
        protected Connection OutputCnx, DurationCnx, DelayCnx, EaseCnx, TargetCnx, LoopCnx;
        [SerializeField]
        private List<Connection> Events = new List<Connection>();
        [SerializeField]
        protected List<Connection> InputCnx = new List<Connection>();

        Connection IDeterministicTweenChain.OutputCnx => OutputCnx;
        public override List<Connection> Inputs
        {
            get
            {
                List<Connection> list = new List<Connection>(InputCnx);
                if (DurationCnx != null) list.Add(DurationCnx);
                if (DelayCnx != null) list.Add(DelayCnx);
                if (EaseCnx != null) list.Add(EaseCnx);
                if (TargetCnx != null) list.Add(TargetCnx);
                return list;
            }
        }
        public override List<Connection> Outputs
        {
            get
            {
                List<Connection> list = new List<Connection>(1);
                if (OutputCnx != null) list.Add(OutputCnx);
                return list;
            }
        }

        public abstract Tween GenerateTween();

        /// <summary>
        /// Sets the object's value to before it was tweened.
        /// </summary>
        public abstract void SetDefaultValue();

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            Connection cnx;
            switch (portName)
            {
                case "Duration":
                    if (DurationCnx == null) DurationCnx = CreateInstance<Connection>();
                    cnx = DurationCnx;
                    break;
                case "Delay":
                    if (DelayCnx == null) DelayCnx = CreateInstance<Connection>();
                    cnx = DelayCnx;
                    break;
                case "Ease":
                    if (EaseCnx == null) EaseCnx = CreateInstance<Connection>();
                    cnx = EaseCnx;
                    break;
                case "Target":
                    if (TargetCnx == null) TargetCnx = CreateInstance<Connection>();
                    cnx = TargetCnx;
                    break;
                case "Input":
                    cnx = CreateInstance<Connection>();
                    InputCnx.Add(cnx);
                    break;
                case "Output":
                    if (OutputCnx == null) OutputCnx = CreateInstance<Connection>();
                    cnx = OutputCnx;
                    break;
                case "Loops":
                    if (LoopCnx == null) LoopCnx = CreateInstance<Connection>();
                    cnx = LoopCnx;
                    break;
                default:
                    return false;
            }
            cnx.SetVals(portName, cnxPort, vertex);
            return true;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            switch (portName)
            {
                case "Duration":
                    if (DurationCnx != null && DurationCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        DurationCnx.DestroyCnx();
                        DurationCnx = null;
                        return true;
                    }
                    break;
                case "Delay":
                    if (DelayCnx != null && DelayCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        DelayCnx.DestroyCnx();
                        DelayCnx = null;
                        return true;
                    }
                    break;
                case "Ease":
                    if (EaseCnx != null && EaseCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        EaseCnx.DestroyCnx();
                        EaseCnx = null;
                        return true;
                    }
                    break;
                case "Target":
                    if (TargetCnx != null && TargetCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        TargetCnx.DestroyCnx();
                        TargetCnx = null;
                        return true;
                    }
                    break;
                case "Input":
                    Connection input; 
                    if ((input = InputCnx.Find(cnx => cnx.ValsMatch(portName, cnxPort, vertex))) != null)
                    {
                        input.DestroyCnx();
                        InputCnx.Remove(input);
                        return true;
                    }
                    break;
                case "Output":
                    if(OutputCnx != null && OutputCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        OutputCnx.DestroyCnx();
                        OutputCnx = null;
                        return true;
                    }
                    break;
                case "Loops":
                    if(LoopCnx != null && LoopCnx.ValsMatch(portName, cnxPort, vertex))
                    {
                        LoopCnx.DestroyCnx();
                        LoopCnx = null;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public virtual void Propogate(Vertex propogator, Vertex target, string port)
        {
            Connection cnx = port switch
            {
                "Input" => InputCnx.Find(cnx => cnx.Cnx == propogator && cnx.PortName == port),
                "Output" => OutputCnx,
                "Duration" => DurationCnx,
                "Delay" => DelayCnx,
                "Ease" => EaseCnx,
                "Target" => TargetCnx,
                "Loops" => LoopCnx,
                _ => null
            };
            if (cnx != null) cnx.TargetCnx = target;
        }

        /// <summary>
        /// Sets duration and delay variables for tweens.
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="delay"></param>
        protected void GetDurationDelay(out float duration, out float delay)
        {
            duration = DurationCnx == null ? 0f : ((ValueVertex<float>)DurationCnx.TargetCnx).Value;
            delay = DelayCnx == null ? 0f : ((ValueVertex<float>)DelayCnx.TargetCnx).Value;
        }

        /// <summary>
        /// Sets the ease and loops for the tween from the vertex's inputs.
        /// </summary>
        /// <param name="tween"></param>
        protected void SetEaseAndLoops(Tween tween)
        {
            int loops = LoopCnx == null ? 0 : ((ValueVertex<int>)LoopCnx.TargetCnx).Value;
            tween.SetLoops(loops);

            if (EaseCnx == null)
            {
                tween.SetEase(Ease.Unset);
            }
            else if (EaseCnx.TargetCnx is AnimationCurveValueVertex curvVertex)
            {
                tween.SetEase(curvVertex.Value);
            }
            else if(EaseCnx.TargetCnx is EaseValueVertex easeVertex)
            {
                tween.SetEase(easeVertex.Value);
            }
        }
    
        /// <summary>
        /// Sets any events for the tween from the vertex's inputs.
        /// </summary>
        /// <param name="tween"></param>
        protected void SetEvents(Tween tween)
        {
            foreach(Connection evtCnx in Events)
            {
                ExposedEvent exposed = ((EventVertex)evtCnx.TargetCnx).ExposedEvent;
                switch (evtCnx.TargetCnx)
                {
                    case OnStartEventVertex:
                        tween.OnStart(exposed.Invoke);
                        break;
                    case OnPlayEventVertex:
                        tween.OnPlay(exposed.Invoke);
                        break;
                    case OnCompleteEventVertex:
                        tween.OnComplete(exposed.Invoke);
                        break;
                    case OnKillEventVertex:
                        tween.OnKill(exposed.Invoke);
                        break;
                    case OnPauseEventVertex:
                        tween.OnPause(exposed.Invoke);
                        break;
                    case OnRewindEventVertex:
                        tween.OnRewind(exposed.Invoke);
                        break;
                    case OnStepCompleteEventVertex:
                        tween.OnStepComplete(exposed.Invoke);
                        break;
                    case OnUpdateEventVertex:
                        tween.OnUpdate(exposed.Invoke);
                        break;
                    case OnWaypointChangeEventVertex:
                        tween.OnWaypointChange(index => exposed.Invoke(new object[1] { index }));
                        break;
                    default:
                        Debug.LogError("Unknown Event: " + evtCnx.TargetCnx.GetType().Name);
                        break;
                }
            }
        }

        /// <summary>
        /// Get the target connection.
        /// </summary>
        /// <param name="defaultTarget">The default target.</param>
        /// <typeparam name="T">Type of target to try to get.</typeparam>
        /// <returns>Null if no target connected, otherwise tries to return an Object of type T.</returns>
        protected T GetTarget<T>(Object defaultTarget)
        {
            Object target = TargetCnx == null ? defaultTarget : ((ValueVertex<Object>)TargetCnx.TargetCnx).Value;

            if (target is T type)
            {
                return type;
            }
            if (target is GameObject obj)
            {
                return obj.GetComponent<T>();
            }
            if(target is Transform trans)
            {
                return trans.GetComponent<T>();
            }
            else
            {
                throw new InvalidCastException("Unable to obtain appropriate target");
            }
        }
    }
}
