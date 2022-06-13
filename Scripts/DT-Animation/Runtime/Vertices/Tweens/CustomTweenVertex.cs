using DG.Tweening;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    [NodeMenuDisplay("Tweens/Custom")]
    public class CustomTweenVertex : TweenVertex
    {
        public ExposedMethod ExposedMethod;

        public override Tween GenerateTween() => ExposedMethod?.Invoke<Tween>();

        public override void SetDefaultValue() { }
    }
}
