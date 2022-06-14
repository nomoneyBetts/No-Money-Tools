// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using UnityEngine;

namespace NoMoney
{
    [System.Serializable]
    public class ExposedMethod
    {
        [SerializeField]
        internal Object Target;
        [SerializeField]
        internal string Method;
        [SerializeField]
        internal List<MethodParameter> Parameters = new List<MethodParameter>();

        /// <summary>
        /// Invoke the exposed method.
        /// </summary>
        public void Invoke()
        {
            if (NullChecks()) return;
            Target.GetMethod(Method)?.Invoke(Target, Parameters.ToObjArray());
        }

        /// <summary>
        /// Invoke the exposed method.
        /// </summary>
        /// <param name="parameters">Parameters to use instead of the exposed parameters.</param>
        public void Invoke(object[] parameters)
        {
            if (NullChecks()) return;
            Target.GetMethod(Method)?.Invoke(Target, parameters);
        }

        /// <summary>
        /// Invoke the exposed method.
        /// </summary>
        /// <typeparam name="T">The method's return type.</typeparam>
        /// <returns>An object of type T.</returns>
        public T Invoke<T>()
        {
            if (NullChecks()) return default;
            return (T)Target.GetMethod(Method)?.Invoke(Target, Parameters.ToObjArray());
        }

        /// <summary>
        /// Invoke the exposed method.
        /// </summary>
        /// <typeparam name="T">The method's return type.</typeparam>
        /// <param name="parameters">Parameters to use instead of the exposed parameters.</param>
        /// <returns>An object of type T.</returns>
        public T Invoke<T>(object[] parameters)
        {
            if (NullChecks()) return default;
            return (T)Target.GetMethod(Method)?.Invoke(Target, parameters);
        }

        /// <summary>
        /// Checks if any values are null.
        /// </summary>
        /// <returns>True if values are null; False otherwise.</returns>
        private bool NullChecks()
        {
            bool isNull = false;
            if(Target == null)
            {
                Debug.LogWarning("ExposedMethod Target is Unset");
                isNull = true;
            }
            if (string.IsNullOrEmpty(Method))
            {
                Debug.LogWarning("ExposedMethod Method is Unset");
                isNull = true;
            }
            return isNull;
        }
    }
}
