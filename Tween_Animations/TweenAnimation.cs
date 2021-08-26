using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace TweeningAnimations{
    /*
    Some common tween types I use.
    */
    public enum TweenType{ Unset, Scale, FadeTMPro, FadeSprite, Move, LocalMove }
    /*
    All Scripts used for tweening should inherit from Tween Animation
    */
    public abstract class TweenAnimation : MonoBehaviour
    {
#region ParameterClasses
        public abstract class ParamterClass{
            protected System.Exception unsetTweenType = 
            new System.Exception("Tween Type is Unset");
            protected System.Exception unknownTweenType = 
            new System.Exception("Tween Type is Unknown");
            protected System.Exception incompatibleTweenType = 
            new System.Exception("Tween Type is Incompatible");

            public TweenType tweenType;
            public float duration, delay;
            public Ease ease;
        }

        [System.Serializable]
        public class TweenFloatParameters : ParamterClass{
            public float start, end;

            /*
            Generates a Tween of the selected type.
            */
            public Tween GenerateTween(GameObject obj){
                Tween tween;
                switch(tweenType){
                    case TweenType.Unset:
                    throw unsetTweenType;
                    case TweenType.Scale:
                    tween = obj.transform
                        .DOScale(end, duration)
                        .From(start)
                        .SetDelay(delay)
                        .SetEase(ease);
                    break;
                    case TweenType.FadeTMPro:
                    tween = obj.GetComponent<TMPro.TMP_Text>()
                        .DOFade(end, duration)
                        .From(start)
                        .SetDelay(delay)
                        .SetEase(ease);
                    break;
                    case TweenType.FadeSprite:
                    tween = obj.GetComponent<SpriteRenderer>()
                        .DOFade(end, duration)
                        .From(start)
                        .SetDelay(delay)
                        .SetEase(ease);
                    break;
                    case TweenType.LocalMove:
                    case TweenType.Move:
                    throw incompatibleTweenType;
                    default:
                    throw unknownTweenType;
                }
                return tween;
            }
        }

        [System.Serializable]
        public class TweenVecParameters : ParamterClass{
            public Vector3 start, end;

            /*
            Generates a Tween of the selected type.
            */
            public Tween GenerateTween(GameObject obj){
                Tween tween;
                switch(tweenType){
                    case TweenType.Unset:
                    throw unsetTweenType;
                    // neither fade will work with vectors
                    case TweenType.FadeTMPro:
                    case TweenType.FadeSprite:
                    throw incompatibleTweenType;
                    case TweenType.Move:
                    tween = obj.transform
                        .DOMove(end, duration)
                        .From(start)
                        .SetDelay(delay)
                        .SetEase(ease);
                    break;
                    case TweenType.LocalMove:
                    tween = obj.transform
                        .DOLocalMove(end, duration)
                        .From(start)
                        .SetDelay(delay)
                        .SetEase(ease);
                    break;
                    case TweenType.Scale:
                    tween = obj.transform
                        .DOScale(end, duration)
                        .From(start)
                        .SetDelay(delay)
                        .SetEase(ease);
                    break;
                    default:
                    throw unknownTweenType;
                }
                return tween;
            }
        }
#endregion

        public Sequence currentSequence{ get; protected set; }
        public Tween currentTween{ get; protected set; }
        // make sure to set this in inherited class
        public bool isTween{ get; protected set; }

        public IEnumerator WaitForCurrent(Action onComplete){
            if(isTween && currentTween != null){
                yield return currentTween.WaitForCompletion();
            }
            else if(!isTween && currentSequence != null){
                yield return currentSequence.WaitForCompletion();
            }
            if(onComplete != null)
                onComplete();
        }

        public void KillCurrent(){
            if(currentSequence != null){
                currentSequence.Kill();
                currentSequence = null;
            }
            if(currentTween != null){
                currentTween.Kill();
                currentTween = null;
            }
        }

        public void PlayCurrent(){
            if(isTween)
                currentTween.Play();
            else
                currentSequence.Play();
        }

        public void RestartCurrent(){
            if(isTween)
                currentTween.Restart();
            else
                currentSequence.Restart();
        }

        public bool IsCurrentPlaying(){
            if(isTween)
                return currentTween.IsPlaying();
            else 
                return currentSequence.IsPlaying();
        }
    }
}