// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using UnityEngine;

namespace NoMoney
{
    [System.Serializable]
    public class ExposedEvent
    {
        [SerializeField]
        public List<ExposedMethod> Methods = new List<ExposedMethod>();

        /// <summary>
        /// Invoke the exposed event.
        /// </summary>
        public void Invoke() => Methods.ForEach(method => method?.Invoke());

        /// <summary>
        /// Invoke the exposed event.
        /// </summary>
        /// <param name="parameters">Parameters to give each method.</param>
        public void Invoke(object[] parameters) => Methods.ForEach(method => method?.Invoke(parameters));
    }
}
