// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

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

        /// <summary>
        /// Start a sequence with the given name.
        /// </summary>
        /// <param name="sequenceName">The name of the sequence to start.</param>
        public void StartSequence(string sequenceName)
        {
            Sequence sequence = _sequenceBook[sequenceName].IsActive() ?
                _sequenceBook[sequenceName] : GenerateSequence(sequenceName);
            if (sequence == null) Debug.LogError("Unable to find Sequence by name: " + sequenceName);
            else sequence.Restart();
        }

        /// <summary>
        /// Toggles pause on the sequence with the given name.
        /// </summary>
        /// <param name="sequenceName">The name of the sequence to pause.</param>
        public void TogglePause(string sequenceName)
        {
            if (_sequenceBook[sequenceName].IsActive())
            {
                _sequenceBook[sequenceName].TogglePause();
            }
        }

        /// <summary>
        /// Retrieve a stored DoTween.Sequence
        /// </summary>
        /// <param name="sequenceName">The name of the sequence to retrieve.</param>
        /// <returns>DoTween.Sequence</returns>
        public Sequence GetSequence(string sequenceName) => _sequenceBook[sequenceName];

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
    }
}
