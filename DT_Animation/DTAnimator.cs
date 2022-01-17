using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DT_Animation
{
    public class DTAnimator : MonoBehaviour
    {
        public SequencerData data;
        public Dictionary<string, Sequence> sequences;

        private void Awake()
        {
            sequences = new Dictionary<string, Sequence>();
            GenerateSequences();
        }

        private void GenerateSequences()
        {
            if(data == null)
            {
                return;
            }

            Sequence prevSequence = null;
            foreach(DTSequence dtSequence in data.sequences)
            {
                if (dtSequence.noAutoGenerate)
                {
                    continue;
                }

                Sequence curSequence = GenerateSequence(dtSequence);
                if (dtSequence.isJoined)
                {
                    prevSequence.Join(curSequence);
                }
                sequences.Add(dtSequence.name, curSequence);
                prevSequence = curSequence;
            }
        }

        private Sequence GenerateSequence(DTSequence dtSequence)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .SetDelay(dtSequence.delay)
                .SetAutoKill(false)
                .SetLoops(dtSequence.loops);
                //.OnStart(dtSequence.onStart.Invoke)
                //.OnPlay(dtSequence.onPlay.Invoke)
                //.OnPause(dtSequence.onPause.Invoke)
                //.OnComplete(dtSequence.onComplete.Invoke);

            foreach(DTTween dtTween in dtSequence.tweens)
            {
                Tween tween = GenerateTween(dtTween);
                if (dtTween.isJoined)
                {
                    sequence.Join(tween);
                }
                else
                {
                    sequence.Append(tween);
                }
            }

            return sequence;
        }

        private Tween GenerateTween(DTTween dtTween)
        {
            Tween tween;
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Vector3 scale = transform.localScale;

            switch (dtTween.tweenType)
            {
                case TweenType.Move:
                    tween = transform
                        .DOMove(dtTween.endVec, dtTween.duration)
                        .From(dtTween.startVec);
                    break;
                case TweenType.LocalMove:
                    tween = transform
                        .DOLocalMove(dtTween.endVec, dtTween.duration)
                        .From(dtTween.startVec);
                    break;
                case TweenType.Rotate:
                    tween = transform
                        .DORotate(dtTween.endVec, dtTween.duration)
                        .From(dtTween.startVec);
                    break;
                case TweenType.LocalRotate:
                    tween = transform
                        .DOLocalRotate(dtTween.endVec, dtTween.duration)
                        .From(dtTween.startVec);
                    break;
                case TweenType.Scale:
                    tween = transform
                        .DOScale(dtTween.endVec, dtTween.duration)
                        .From(dtTween.startVec);           
                    break;
                //case TweenType.Fade:
                //    break;
                default:
                    throw new System.Exception("Failed to Generate Tween. Tween Type: " + dtTween.tweenType);
            }

            if(dtTween.ease == Ease.INTERNAL_Custom)
            {
                tween.SetEase(dtTween.customCurve);
            }
            else
            {
                tween.SetEase(dtTween.ease);
            }

            tween
                .SetLoops(dtTween.loops)
                .SetAutoKill(false);
                //.OnPlay(dtTween.onPlay.Invoke)
                //.OnPause(dtTween.onPause.Invoke)
                //.OnStart(dtTween.onStart.Invoke)
                //.OnComplete(dtTween.onComplete.Invoke);

            transform.position = pos;
            transform.rotation = rot;
            transform.localScale = scale;

            return tween;
        }
    
        public void Play(string name)
        {
            if (sequences.ContainsKey(name))
            {
                sequences[name].Restart();
            }
        }
    
        public void TogglePause(string name)
        {
            if (sequences.ContainsKey(name))
            {
                sequences[name].TogglePause();
            }
        }
    }
}
