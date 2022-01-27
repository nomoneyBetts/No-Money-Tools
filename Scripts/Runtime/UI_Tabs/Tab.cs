using UnityEngine;
using UnityEngine.UI;

namespace UI_Tabs
{
    /// <summary>
    /// Used to select Tab Menus.
    /// </summary>
    public abstract class Tab : MonoBehaviour
    {
        [SerializeField]
        private TabMenu _menu;
        public TabMenu menu
        {
            get { return _menu; }
            protected set { _menu = value; }
        }

        [SerializeField]
        protected TabWindow window;

        protected virtual void Awake()
        {
            Button button = GetComponent<Button>();
            if(button == null)
            {
                button = gameObject.AddComponent<Button>();
            }

            button.onClick.AddListener(Select);
        }

        /// <summary>
        /// Select this tab.
        /// </summary>
        public virtual void Select()
        {
            window.SelectTab(this);
        }

        /// <summary>
        /// Deselect this tab.
        /// </summary>
        public abstract void Deselect();
    }
}
