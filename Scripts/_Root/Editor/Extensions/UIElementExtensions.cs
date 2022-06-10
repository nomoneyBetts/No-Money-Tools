using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NoMoney
{
    public static class UIElementExtensions
    {
        /// <summary>
        /// Adds placeholder text to a textfield.
        /// </summary>
        public static void SetPlaceHolderText(this TextField textField, string placeholder)
        {
            string placeHolderClass = TextField.ussClassName + "-placeholder";

            onFocusOut();
            textField.RegisterCallback<FocusInEvent>(e => onFocusIn());
            textField.RegisterCallback<FocusOutEvent>(e => onFocusOut());

            void onFocusIn()
            {
                if (textField.ClassListContains(placeHolderClass))
                {
                    textField.value = string.Empty;
                    textField.RemoveFromClassList(placeHolderClass);
                }
            }

            void onFocusOut()
            {
                if (string.IsNullOrEmpty(textField.text))
                {
                    textField.SetValueWithoutNotify(placeholder);
                    textField.AddToClassList(placeHolderClass);
                }
            }
        }

        /// <summary>
        /// Clears text and sets the placeholder.
        /// </summary>
        public static void ClearToPlaceHolder(this TextField textField, string placeHolder)
        {
            string placeHolderClass = TextField.ussClassName + "-placeholder";
            if (!textField.ClassListContains(placeHolderClass))
            {
                textField.SetValueWithoutNotify(placeHolder);
                textField.AddToClassList(placeHolderClass);
            }
        }

        /// <summary>
        /// Sets text without notify, and removes from the placeholder class
        /// </summary>
        public static void SetText(this TextField textField, string text)
        {
            textField.SetValueWithoutNotify(text);
            textField.RemoveFromClassList(TextField.ussClassName + "-placeholder");
        }

        /// <returns>All the ports attached to a node.</returns>
        public static List<Port> GetPorts(this Node node)
        {
            List<Port> ports = new List<Port>();
            VisualElement inputContainer = node.Q<VisualElement>("input");
            if (inputContainer != null)
            {
                foreach (VisualElement e in inputContainer.Children())
                {
                    if (e is Port p)
                    {
                        ports.Add(p);
                    }
                }
            }

            VisualElement outputContainer = node.Q<VisualElement>("output");
            // I renamed Value Node output to value-output for uss styling purposes.
            if (outputContainer == null)
            {
                outputContainer = node.Q<VisualElement>("value-output");
            }
            if (outputContainer != null)
            {
                foreach (VisualElement e in outputContainer.Children())
                {
                    if (e is Port p)
                    {
                        ports.Add(p);
                    }
                }
            }
            return ports;
        }

        /// <summary>
        /// Clears all actions from a ToolbarMenu.
        /// </summary>
        public static void ClearMenu(this ToolbarMenu toolbarMenu)
        {
            int count = toolbarMenu.menu.MenuItems().Count;
            for (int i = 0; i < count; i++)
            {
                toolbarMenu.menu.RemoveItemAt(0);
            }
        }
    }
}
