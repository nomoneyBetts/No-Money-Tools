// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney
{
    /// <summary>
    /// Derive to create a Singleton.
    /// </summary>
    /// <typeparam name="T">The class name of the child object.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour
        where T : Component
    {
        private static T s_instance;
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    T single = FindObjectOfType<T>();
                    if (single == null)
                    {
                        GameObject g = new()
                        {
                            name = typeof(T).Name,
                            hideFlags = HideFlags.HideAndDontSave
                        };
                        s_instance = g.AddComponent<T>();
                    }
                    else
                    {
                        s_instance = single;
                    }
                }
                return s_instance;
            }
            set
            {
                s_instance = value;
            }
        }

        private void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
        }
    }
}
