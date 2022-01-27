using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DT_Animation
{
    public static class ExtensionMethods
    {
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

        public static void ClearToPlaceHolder(this TextField textField, string placeHolder)
        {
            string placeHolderClass = TextField.ussClassName + "-placeholder";
            if (!textField.ClassListContains(placeHolderClass))
            {
                textField.SetValueWithoutNotify(placeHolder);
                textField.AddToClassList(placeHolderClass);
            }
        }

        public static void SetText(this TextField textField, string text)
        {
            textField.SetValueWithoutNotify(text);
            textField.RemoveFromClassList(TextField.ussClassName + "-placeholder");
        }

        public static T SearchChildren<T>(this VisualElement visualElement, string name) where T : VisualElement
        {
            foreach(VisualElement child in visualElement.Children())
            {
                if(child.childCount > 0)
                {
                    T found = SearchChildren<T>(child, name);
                    if (found != null)
                    {
                        return found;
                    }
                }
                if(child is T tChild && child.name == name)
                {
                    return tChild;
                }
            }
            return null;
        }
    }
}
