// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RectGetterNode : GetterNode<Rect>
    {
        public RectGetterNode(RectGetterVertex vertex) : base(vertex) { }
    }
}
