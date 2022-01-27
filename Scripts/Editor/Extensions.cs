using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

internal static class Extensions
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

    /// <summary>
    /// Searches through Visual Tree for an element matching type and name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="visualElement"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T SearchChildren<T>(this VisualElement visualElement, string name) where T : VisualElement
    {
        foreach (VisualElement child in visualElement.Children())
        {
            if (child.childCount > 0)
            {
                T found = SearchChildren<T>(child, name);
                if (found != null)
                {
                    return found;
                }
            }
            if (child is T tChild && child.name == name)
            {
                return tChild;
            }
        }
        return null;
    }

    /// <summary>
    /// Checks if a directory is at the path, otherwise makes it.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>True if path already existed, false if had to create.</returns>
    public static bool ConfirmDirectoryExists(this string path)
    {
        bool exists = Directory.Exists(path);
        if (!exists)
        {
            int i;
            for(i = path.Length - 1; i > -1; i--)
            {
                if(path[i] == '\\')
                {
                    break;
                }
            }
            string parent = path.Remove(i);
            DirectoryInfo info = new DirectoryInfo(parent);
            AssetDatabase.CreateFolder(info.Parent.FullName.TrimToAssets(), info.Name);
        }
        return exists;
    }

    public static string TrimToAssets(this string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);
        while(info.Name != "Assets")
        {
            info = new DirectoryInfo(info.Parent.FullName);
        }

        string trimmed = $"Assets\\{path.Replace(info.FullName, "")}";
        return trimmed;
    }
}
