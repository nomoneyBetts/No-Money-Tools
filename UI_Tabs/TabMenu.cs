using UnityEngine;

namespace UI_Tabs
{
    /// <summary>
    /// A menu in the Tab Window.
    /// </summary>
    public class TabMenu : MonoBehaviour
    {
        /// <summary>
        /// Select this menu.
        /// </summary>
        public virtual void Select()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deselect this menu.
        /// </summary>
        public virtual void Deselect()
        {
            gameObject.SetActive(false);
        }
    }
}
