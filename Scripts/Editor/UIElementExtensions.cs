using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.NoMoneyEditor
{
    public static class UIElementExtensions
    {
        /// <summary>
        /// Adds placeholder text to a textfield.
        /// </summary>
        /// <param name="textField"></param>
        /// <param name="placeholder"></param>
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
        /// <param name="textField"></param>
        /// <param name="placeHolder"></param>
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
        /// <param name="textField"></param>
        /// <param name="text"></param>
        public static void SetText(this TextField textField, string text)
        {
            textField.SetValueWithoutNotify(text);
            textField.RemoveFromClassList(TextField.ussClassName + "-placeholder");
        }

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
    }
}
