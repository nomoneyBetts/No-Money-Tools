using UnityEngine;

namespace NoMoney_Core
{
    /// <summary>
    /// Singelton objects inherit from this class.
    /// </summary>
    /// <typeparam name="T">The class name of the child object.</typeparam>
    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        private static T Instance;
        public static T instance
        {
            get
            {
                if (Instance == null)
                {
                    T single = FindObjectOfType<T>();
                    if (single == null)
                    {
                        GameObject g = new GameObject();
                        g.name = typeof(T).Name;
                        g.hideFlags = HideFlags.HideAndDontSave;
                        Instance = g.AddComponent<T>();
                    }
                    else
                    {
                        Instance = single;
                    }
                }
                return Instance;
            }
            set
            {
                Instance = value;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
