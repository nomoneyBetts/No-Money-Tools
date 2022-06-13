using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.UITabs
{
    /// <summary>
    /// The container for holding tabs.
    /// </summary>
    public class TabContainer : MonoBehaviour
    {
        [HideInInspector]
        public TabMenu Menu;
        public bool ShowAddButtonWarning = true;

        public GameObject[] Tabs
        {
            get
            {
                GameObject[] tabs = new GameObject[transform.childCount];
                for(int i = 0; i < tabs.Length; i++)
                {
                    tabs[i] = transform.GetChild(i).gameObject;
                }
                return tabs;
            }
        }

        private void Awake()
        {
            int count = 0;
            foreach(GameObject tab in Tabs)
            {
                Button button = tab.GetComponent<Button>();
                if (button == null)
                {
                    if(ShowAddButtonWarning) Debug.LogWarning($"Adding button to Tab: {count}");
                    button = tab.AddComponent<Button>();
                }
                button.onClick.AddListener(() => Menu.SelectTab(GetTabIndex(tab.transform)));
                count++;
            }
        }

        private int GetTabIndex(Transform tab)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i) == tab) return i;
            }
            return -1;
        }
    }
}
