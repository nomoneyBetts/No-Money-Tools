// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class AnimationCurveGetterNode : GetterNode<AnimationCurve>
    {
        public AnimationCurveGetterNode(AnimationCurveGetterVertex vertex) : base(vertex) { }
    }
}
