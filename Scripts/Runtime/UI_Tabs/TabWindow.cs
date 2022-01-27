using UnityEngine;

namespace UI_Tabs
{
    /// <summary>
    /// Manages a set of tabs and windows.
    /// </summary>
    public class TabWindow : MonoBehaviour
    {
        public GameObject tabGroup, menuGroup;
        public Tab activeTab { get; protected set; }
        public TabMenu activeMenu { get; protected set; }
        
        protected Tab[] tabs;
        protected TabMenu[] menus;

        protected virtual void Awake()
        {
            tabs = tabGroup.GetComponentsInChildren<Tab>(true);
            menus = menuGroup.GetComponentsInChildren<TabMenu>(true);
        }

        /// <summary>
        /// Clears current selection, sets new active tab and menu.
        /// </summary>
        /// <param name="tab">The tab to select.</param>
        public virtual void SelectTab(Tab tab)
        {
            ClearSelection();
            activeTab = tab;
            activeMenu = tab.menu;
            activeMenu.Select();
        }

        /// <summary>
        /// Deselects active tab and menu, then sets them to null.
        /// </summary>
        public virtual void ClearSelection()
        {
            if (activeMenu != null)
            {
                activeMenu.Deselect();
            }
            if (activeTab != null)
            {
                activeTab.Deselect();
            }
            activeTab = null;
            activeMenu = null;
        }
    }
}
