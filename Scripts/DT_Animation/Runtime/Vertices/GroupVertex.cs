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
