using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace DT_Animation
{
    public class SequencerData : ScriptableObject
    {
        public new string name;
        public DTSequence[] sequences;
    }

    [System.Serializable]
    public class DTSequence
    {
        [HideInInspector]
        public string name;
        [HideInInspector]
        public float delay;
        [HideInInspector]
        public bool isJoined, noAutoGenerate;
        [HideInInspector]
        public int loops;
    
        public List<DTTween> tweens;
        //public UnityEvent onPlay, onStart, onPause, onComplete;
    }

    [System.Serializable]
    public class DTTween
    {
        [HideInInspector]
        public TweenType tweenType;
        [HideInInspector]
        public float duration, delay;
        [HideInInspector]
        public Ease ease;
        [HideInInspector]
        public bool isJoined, isFloat;
        [HideInInspector]
        public int loops;

        [HideInInspector]
        public Vector3 startVec, endVec;
        [HideInInspector]
        public float startVal, endVal;

        [HideInInspector]
        public AnimationCurve customCurve;
        //public UnityEvent onPlay, onStart, onPause, onComplete;
    }

    public enum TweenType
    {
        Unset,
        Move,
        LocalMove,
        Fade,
        Scale,
        Rotate,
        LocalRotate
    }
}
