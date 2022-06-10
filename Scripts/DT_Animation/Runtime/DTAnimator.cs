using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    [System.Serializable]
    internal class StringVertexDict : SerializableDictionary<string, SequenceVertex> { }

    public class DTAnimator : MonoBehaviour
    {
        #region Serialization
        [SerializeField]
        private List<Vertex> _vertices;
        [SerializeField]
        private List<GroupVertex> _groups;
        [SerializeField]
        private StringVertexDict _seqVertices = new StringVertexDict();
        #endregion

        private Dictionary<string, Sequence> _sequenceBook;

        private void Awake()
        {
            if (_sequenceBook == null) GenerateSequences();
        }

        private void GenerateSequences()
        {
            _sequenceBook = new Dictionary<string, Sequence>();
            foreach (SequenceVertex seqVertex in _seqVertices.Values)
            {
                _sequenceBook.Add(seqVertex.SequenceName, null);
                if (!seqVertex.DynamicGeneration)
                {
                    Sequence sequence = GenerateSequence(seqVertex.SequenceName);
                    sequence.SetAutoKill(false);
                    _sequenceBook[seqVertex.SequenceName] = sequence;
                }
            }
        }

        private Sequence GenerateSequence(string name)
        {
            SequenceVertex seqVertex = _seqVertices[name];
            return seqVertex == null ? null : seqVertex.GenerateSequence();
        }

        public void StartSequence(string sequenceName)
        {
            Sequence sequence = _sequenceBook[sequenceName].IsActive() ?
                _sequenceBook[sequenceName] : GenerateSequence(sequenceName);
            sequence.Restart();
        }
    }
}
