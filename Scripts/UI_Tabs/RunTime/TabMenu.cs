using UnityEngine;

namespace NoMoney.UITabs
{
    /// <summary>
    /// A Menu holds Windows and Tabs.
    /// There must be a child group for windows and a child group for tabs.
    /// </summary>
    public class TabMenu : MonoBehaviour
    {
        public ExposedEvent OnTabSelect, OnTabDeselect, OnWindowOpen, OnWindowClose, OnMenuOpen, OnMenuClose;
        public int LandingTab;
        public bool PersistentTabs = false;
        public bool DeactivateWindowOnClose = true;
        public bool DeactivateMenuOnClose = true;

        private TabWindowContainer _windows;
        private TabContainer _tabs;
        private int _selectedTab = -1;

        private void Awake()
        {
            _windows = GetComponentInChildren<TabWindowContainer>(false);
            if (_windows == null)
            {
                throw new MissingComponentException("Unable to find TabWindowContainer on active children");
            }
            _windows.Menu = this;
            _tabs = GetComponentInChildren<TabContainer>(false);
            if (_tabs == null)
            {
                throw new MissingComponentException("Unable to find TabContainer on active children");
            }
            _tabs.Menu = this;

            int tabLength = _tabs.Tabs.Length;
            int windowLength = _windows.Windows.Length;
            if (_windows.Windows.Length != _tabs.Tabs.Length)
            {
                throw new System.Exception($"Unable to map tabs to windows: Tabs: {tabLength}, Windows: {windowLength}.");
            }
        }

        private void OnEnable()
        {
            _windows.DeactivateWindows();
        }

        /// <summary>
        /// Select a tab and display its window.
        /// </summary>
        /// <param name="index">The index of the tab in the container heirarchy.</param>
        public void SelectTab(int index)
        {
            if (_selectedTab > -1)
            {
                object[] oldTab = new object[2] { _selectedTab, _tabs };
                object[] oldWindow = new object[2] { _selectedTab, _windows };
                OnTabDeselect.Invoke(oldTab);
                OnWindowClose.Invoke(oldWindow);
                if (DeactivateWindowOnClose) _windows.Windows[_selectedTab].SetActive(false);
            }

            object[] newTab = new object[2] { index, _tabs };
            object[] newWindow = new object[2] { index, _windows };
            OnTabSelect.Invoke(newTab);
            _windows.Windows[index].SetActive(true);
            OnWindowOpen.Invoke(newWindow);

            _selectedTab = index;
        }

        public void CloseMenu()
        {
            object[] menu = new object[1] { this };
            OnMenuClose.Invoke(menu);
            if (DeactivateMenuOnClose) gameObject.SetActive(false);
        }

        public void OpenMenu()
        {
            gameObject.SetActive(true);
            object[] menu = new object[1] { this };
            OnMenuOpen.Invoke(menu);
            if (PersistentTabs && _selectedTab > -1) SelectTab(_selectedTab);
            else SelectTab(LandingTab);
        }
    }
}
