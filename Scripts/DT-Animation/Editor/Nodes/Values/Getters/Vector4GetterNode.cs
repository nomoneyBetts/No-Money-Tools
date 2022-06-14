// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class Vector4GetterNode : GetterNode<Vector4>
    {
        public Vector4GetterNode(Vector4GetterVertex vertex) : base(vertex) { }
    }
}
