using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RotateVertex : TweenVertex
    {
        public override Tween GenerateTween(Transform defaultTrans)
        {
            Quaternion rotation = defaultTrans.rotation;
            GetTweenValues(out float duration, out float delay, out Ease ease);

            Connection input;
            input = Inputs.Find(c => c.PortName == "Start");
            Vector3 start = input == null ? defaultTrans.position : ((Vector3ValueVertex)input.CnxVertex).Value;
            input = Inputs.Find(c => c.PortName == "End");
            Vector3 end = input == null ? defaultTrans.position : ((Vector3ValueVertex)input.CnxVertex).Value;

            Tween tween = defaultTrans
                .DORotate(end, duration)
                .From(start)
                .SetDelay(delay)
                .SetEase(ease)
                .SetAutoKill(false);

            defaultTrans.rotation = rotation;
            return tween;
        }
    }
}
