using UnityEditor;
using UnityEngine.UIElements;
using NoMoney;

public static class INotifyExtensions 
{
    public static void SetAndRegister<T>(this INotifyValueChanged<T> notifier, SerializedObject obj, string field)
    {
        notifier.SetValueWithoutNotify(obj.FindProperty(field).GetValue<T>());
        notifier.RegisterValueChangedCallback(evt => obj.FindProperty(field).SetValue(evt.newValue));
    }
}
