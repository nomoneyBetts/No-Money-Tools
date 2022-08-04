// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class Vector3GetterNode : GetterNode<Vector3>
    {
        public Vector3GetterNode(Vector3GetterVertex vertex) : base(vertex) { }
    }
}
