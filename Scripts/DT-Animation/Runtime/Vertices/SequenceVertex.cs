using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Sequence")]
    public class SequenceVertex : Vertex, IElbowPropogator, IDeterministicTweenChain
    {
        public string SequenceName;
        public bool DynamicGeneration;

        [SerializeField]
        private List<Connection> _events = new List<Connection>();
        [SerializeField]
        private Connection _outputCnx;

        public Connection OutputCnx => _outputCnx;
        public override List<Connection> Outputs
        {
            get
            {
                List<Connection> list = new List<Connection>(1);
                if (_outputCnx != null) list.Add(_outputCnx);
                return list;
            }
        }
        public override List<Connection> Inputs => _events;

        public override bool ConnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (portName == "Events")
            {
                Connection cnx = CreateInstance<Connection>();
                cnx.SetVals(portName, cnxPort, vertex);
                _events.Add(cnx);
                return true;
            }
            else if(portName == "Output")
            {
                if(_outputCnx == null) _outputCnx = CreateInstance<Connection>();
                _outputCnx.SetVals(portName, cnxPort, vertex);
                return true;
            }
            return false;
        }

        public override bool DisconnectVertex(string portName, string cnxPort, Vertex vertex)
        {
            if (portName == "Events")
            {
                Connection cnx = _events.Find(c => c.ValsMatch(portName, cnxPort, vertex));
                if (cnx != null)
                {
                    cnx.DestroyCnx();
                    _events.Remove(cnx);
                }
                return true;
            }
            else if(_outputCnx != null && _outputCnx.ValsMatch(portName, cnxPort, vertex))
            {
                _outputCnx.DestroyCnx();
                _outputCnx = null;
                return true;
            }
            return false;
        }

        public Sequence GenerateSequence()
        {
            Sequence sequence = DOTween.Sequence();
            SetEvents(sequence);
            sequence.SetAutoKill(false);
            GenerationHelper(this, sequence);
            return sequence;

            void SetEvents(Sequence sequence)
            {
                foreach (Connection evtCnx in _events)
                {
                    ExposedEvent exposed = ((EventVertex)evtCnx.TargetCnx).ExposedEvent;
                    switch (evtCnx.TargetCnx)
                    {
                        case OnStartEventVertex:
                            sequence.OnStart(exposed.Invoke);
                            break;
                        case OnPlayEventVertex:
                            sequence.OnPlay(exposed.Invoke);
                            break;
                        case OnCompleteEventVertex:
                            sequence.OnComplete(exposed.Invoke);
                            break;
                        case OnKillEventVertex:
                            sequence.OnKill(exposed.Invoke);
                            break;
                        case OnPauseEventVertex:
                            sequence.OnPause(exposed.Invoke);
                            break;
                        case OnRewindEventVertex:
                            sequence.OnRewind(exposed.Invoke);
                            break;
                        case OnStepCompleteEventVertex:
                            sequence.OnStepComplete(exposed.Invoke);
                            break;
                        case OnUpdateEventVertex:
                            sequence.OnUpdate(exposed.Invoke);
                            break;
                        case OnWaypointChangeEventVertex:
                            sequence.OnWaypointChange(index => exposed.Invoke(new object[1] { index }));
                            break;
                        default:
                            Debug.LogError("Unknown Event: " + evtCnx.TargetCnx.GetType().Name);
                            break;
                    }
                }

            }

            void GenerationHelper(Vertex curVertex, Sequence sequence)
            {
                if (curVertex == null) return;
                if (curVertex is TweenVertex tweVertex)
                {
                    sequence.Append(tweVertex.GenerateTween());
                }
                else if (curVertex is JoinVertex joinVertex)
                {
                    bool isFirst = true;
                    foreach (Connection cnx in joinVertex.Outputs)
                    {
                        Sequence joinSequence = DOTween.Sequence();
                        GenerationHelper(cnx.Cnx, joinSequence);
                        if (isFirst)
                        {
                            isFirst = false;
                            sequence.Append(joinSequence);
                        }
                        else
                        {
                            sequence.Join(joinSequence);
                        }
                    }
                    return;
                }

                if (curVertex is IDeterministicTweenChain link && link.OutputCnx != null)
                {
                    GenerationHelper(link.OutputCnx.Cnx, sequence);
                }
            }
        }

        public void Propogate(Vertex propogator, Vertex target, string port)
        {
            if(port == "Events")
            {
                Connection cnx = _events.Find(c => c.Cnx == propogator);
                if (cnx != null) cnx.TargetCnx = target;
            }
        }
    }
}
