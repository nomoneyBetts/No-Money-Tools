using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NoMoney.UITabs
{
    [CustomEditor(typeof(TabMenu))]
    public class TabMenuEditor : Editor
    {
        private const string LandingTab = "landing-tab";
        private const string PersistentTabs = "persistent-tabs";
        private const string DeactivateWindows = "deactivate-windows";
        private const string DeactivateMenu = "deactivate-menu";

        private const string MenuOpen = "menu-open";
        private const string MenuClose = "menu-close";
        private const string WindowOpen = "window-open";
        private const string WindowClose = "window-close";
        private const string TabSelect = "tab-select";
        private const string TabDeselect = "tab-deselect";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("TabMenu");
            visualTree.CloneTree(inspector);

            #region Settings
            IntegerField intField = inspector.Q<IntegerField>(LandingTab);
            intField.SetValueWithoutNotify((int)serializedObject.FindProperty("LandingTab").GetValue());
            intField.RegisterValueChangedCallback(evt =>
            {
                int value = evt.newValue;
                if(value < 0)
                {
                    value = 0;
                    intField.SetValueWithoutNotify(value);
                }
                serializedObject.FindProperty("LandingTab").SetValue(value);
            });

            Toggle persistence = inspector.Q<Toggle>(PersistentTabs);
            persistence.SetValueWithoutNotify((bool)serializedObject.FindProperty("PersistentTabs").GetValue());
            persistence.RegisterValueChangedCallback(evt => serializedObject.FindProperty("PersistentTabs").SetValue(evt.newValue));

            Toggle deactivate = inspector.Q<Toggle>(DeactivateWindows);
            deactivate.SetValueWithoutNotify((bool)serializedObject.FindProperty("DeactivateWindowOnClose").GetValue());
            deactivate.RegisterValueChangedCallback(evt => serializedObject.FindProperty("DeactivateWindowOnClose").SetValue(evt.newValue));

            deactivate = inspector.Q<Toggle>(DeactivateMenu);
            deactivate.SetValueWithoutNotify((bool)serializedObject.FindProperty("DeactivateMenuOnClose").GetValue());
            deactivate.RegisterValueChangedCallback(evt => serializedObject.FindProperty("DeactivateMenuOnClose").SetValue(evt.newValue));
            #endregion

            #region Events
            EventField field = inspector.Q<EventField>(MenuOpen);
            field.SetValueWithoutNotify((ExposedEvent)serializedObject.FindProperty("OnMenuOpen").GetValue());
            field.RegisterValueChangedCallback(evt => serializedObject.FindProperty("OnMenuOpen").SetValue(evt.newValue));

            field = inspector.Q<EventField>(MenuClose);
            field.SetValueWithoutNotify((ExposedEvent)serializedObject.FindProperty("OnMenuClose").GetValue());
            field.RegisterValueChangedCallback(evt => serializedObject.FindProperty("OnMenuClose").SetValue(evt.newValue));

            field = inspector.Q<EventField>(WindowOpen);
            field.SetValueWithoutNotify((ExposedEvent)serializedObject.FindProperty("OnWindowOpen").GetValue());
            field.RegisterValueChangedCallback(evt => serializedObject.FindProperty("OnWindowOpen").SetValue(evt.newValue));

            field = inspector.Q<EventField>(WindowClose);
            field.SetValueWithoutNotify((ExposedEvent)serializedObject.FindProperty("OnWindowClose").GetValue());
            field.RegisterValueChangedCallback(evt => serializedObject.FindProperty("OnWindowClose").SetValue(evt.newValue));

            field = inspector.Q<EventField>(TabSelect);
            field.SetValueWithoutNotify((ExposedEvent)serializedObject.FindProperty("OnTabSelect").GetValue());
            field.RegisterValueChangedCallback(evt => serializedObject.FindProperty("OnTabSelect").SetValue(evt.newValue));

            field = inspector.Q<EventField>(TabDeselect);
            field.SetValueWithoutNotify((ExposedEvent)serializedObject.FindProperty("OnTabDeselect").GetValue());
            field.RegisterValueChangedCallback(evt => serializedObject.FindProperty("OnTabDeselect").SetValue(evt.newValue));
            #endregion

            return inspector;
        }
    }
}
