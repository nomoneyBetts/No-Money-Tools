using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TweeningAnimations{
    /*
    Some common tween types I use.
    */
    public enum TweenType{ Unset, Scale, FadeTMPro, FadeSprite, Move, LocalMove }
    /*
    All Scripts used for tweening should inherit from Tween Animation.
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

        /*
        Datastructure to hold either a Tween or a Sequence.
        */
        public class Twequence{
            public bool isTween;
            public Tween tween;
            public Sequence sequence;
            public bool isComplete{
                get{
                    if(isTween && tween != null)
                        return tween.IsComplete();
                    else if(!isTween && sequence != null)
                        return sequence.IsComplete();
                    return false;                        
                }
            }

            public Twequence(Tween tween){
                isTween = true;
                this.tween = tween;
            }
            public Twequence(Sequence sequence){
                isTween = false;
                this.sequence = sequence;
            }

            public void Kill(){
                if(isTween)
                    tween.Kill();
                else
                    sequence.Kill();
            }

            public void Play(){
                if(isTween)
                    tween.Play();
                else
                    sequence.Play();
            }

            public void Restart(){
                if(isTween)
                    tween.Restart();
                else
                    sequence.Restart();
            }
        }

        /*
        The current Twequence acting on the Gameobject.
        */
        public Twequence currentTwequence{ get; protected set; }
        
        /*
        A collection of all Twequences acting on the Gameobject.
        */
        private List<Twequence> twequences;

        /*
        Wait for the current Twequence to complete, then perform an Action.
        */
        public IEnumerator WaitForCurrent(Action onComplete){
            if(currentTwequence != null){
                if(currentTwequence.isTween)
                    yield return currentTwequence.tween.WaitForCompletion();
                else
                    yield return currentTwequence.sequence.WaitForCompletion();
            }
            if(onComplete != null)
                onComplete();
        }

        /*
        Kills all the Twequences acting on the Gameobject.
        Probably a good idea to use sparingly, like transitioning between Scenes.
        */
        public void KillAll(){
            if(twequences == null)
                throw new NullReferenceException("Twequences not created prior to killing");

            foreach(Twequence twequence in twequences){
                twequence.Kill();
            }
        }
    
        /*
        Converts a Tween or Sequence into a Twequence.
        */
        protected Twequence MakeTwequence(Tween tween){
            if(twequences == null)
                twequences = new List<Twequence>();
            Twequence t = new Twequence(tween);
            twequences.Add(t);
            return t;
        }
        protected Twequence MakeTwequence(Sequence sequence){
            if(twequences == null)
                twequences = new List<Twequence>();
            Twequence t = new Twequence(sequence);
            twequences.Add(t);
            return t;
        }
    
        /*
        Removes a Twequence from my List.
        Attach to either the OnKill or OnComplete Action.
        */
        protected void RemoveTwequence(Twequence twequence){
            twequences.Remove(twequence);
        }
    }
}
