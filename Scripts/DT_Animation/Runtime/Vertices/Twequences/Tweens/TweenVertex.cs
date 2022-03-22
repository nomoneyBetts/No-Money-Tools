using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    public abstract class TweenVertex : TwequenceVertex
    {
        public abstract Tween GenerateTween(Transform defaultTrans);

        protected void GetTweenValues(out float duration, out float delay, out Ease ease)
        {
            Connection input;
            input = Inputs.Find(c => c.PortName == "Duration");
            duration = input == null ? 0 : ((FloatValueVertex)input.CnxVertex).Value;
            input = Inputs.Find(c => c.PortName == "Delay");
            delay = input == null ? 0 : ((FloatValueVertex)input.CnxVertex).Value;
            input = Inputs.Find(c => c.PortName == "Ease");
            ease = input == null ? Ease.Unset : ((EaseValueVertex)input.CnxVertex).Value;
        }
    }
}
