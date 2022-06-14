// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney.UITabs
{
    /// <summary>
    /// The container for holding windows.
    /// </summary>
    public class TabWindowContainer : MonoBehaviour
    {
        [HideInInspector]
        public TabMenu Menu;
        public GameObject[] Windows
        {
            get
            {
                GameObject[] windows = new GameObject[transform.childCount];
                for (int i = 0; i < windows.Length; i++)
                {
                    windows[i] = transform.GetChild(i).gameObject;
                }
                return windows;
            }
        }

        public void DeactivateWindows()
        {
            foreach(GameObject window in Windows)
            {
                window.SetActive(false);
            }
        }
    }
}
