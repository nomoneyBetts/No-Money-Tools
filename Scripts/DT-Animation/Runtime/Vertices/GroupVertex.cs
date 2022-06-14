// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class GroupVertex : ScriptableObject
    {
        public string Title;
        public List<Vertex> Group = new List<Vertex>();

        public void DestroyVertex() => DestroyImmediate(this);
    }
}
