using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class MoveVertex : TweenVertex
    {
        public override Tween GenerateTween(Transform defaultTrans)
        {
            Vector3 pos = defaultTrans.position;
            GetTweenValues(out float duration, out float delay, out Ease ease);

            Connection input;
            input = Inputs.Find(c => c.PortName == "Start");
            Vector3 start = input == null ? defaultTrans.position : ((Vector3ValueVertex)input.CnxVertex).Value;
            input = Inputs.Find(c => c.PortName == "End");
            Vector3 end = input == null ? defaultTrans.position : ((Vector3ValueVertex)input.CnxVertex).Value;

            Tween tween = defaultTrans
                .DOMove(end, duration)
                .From(start)
                .SetDelay(delay)
                .SetEase(ease)
                .SetAutoKill(false);

            defaultTrans.position = pos;
            return tween;
        }
    }
}
