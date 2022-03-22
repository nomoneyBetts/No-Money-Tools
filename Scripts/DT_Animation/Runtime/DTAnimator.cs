using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NoMoney.DTAnimation {
    public class DTAnimator : MonoBehaviour
    {
        [SerializeField]
        private List<Vertex> _vertices;
        [SerializeField]
        private List<SequenceVertex> _seqVertices;
        private Dictionary<string, Sequence> _sequenceBook;

        private void Awake()
        {
            GenerateSequences();
        }

        private void GenerateSequences()
        {
            _sequenceBook = new Dictionary<string, Sequence>();
            foreach(SequenceVertex seqVertex in _seqVertices)
            {
                _sequenceBook.Add(seqVertex.SequenceName, null);
                if (seqVertex.DynamicGeneration)
                {
                    _sequenceBook[seqVertex.SequenceName].SetAutoKill(true);
                }
                else
                {
                    Sequence sequence = GenerateSequence(seqVertex.SequenceName);
                    sequence.SetAutoKill(false);
                    _sequenceBook[seqVertex.SequenceName] = sequence;
                }
            }
        }
    
        private Sequence GenerateSequence(string name)
        {
            SequenceVertex seqVertex = _seqVertices.Find(v => v.SequenceName == name);
            if(seqVertex == null)
            {
                return null;
            }
            Sequence sequence = DOTween.Sequence();
            TwequenceVertex curVertex = seqVertex;
            while (curVertex != null)
            {
                if (curVertex is TweenVertex tweVertex)
                {
                    sequence.Append(tweVertex.GenerateTween(transform));
                }
                Connection output = curVertex.GetOutput();
                if (output == null)
                {
                    break;
                }
                curVertex = (TwequenceVertex)output.CnxVertex;
            }
            _sequenceBook[name] = sequence;
            return sequence;
        }

        public void StartSequence(string sequenceName)
        {
            Sequence sequence = _sequenceBook[sequenceName].IsActive() ? 
                _sequenceBook[sequenceName] : GenerateSequence(sequenceName);
            sequence.Restart();
        }
    }
}
