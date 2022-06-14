// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

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
