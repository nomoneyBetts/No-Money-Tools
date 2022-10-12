using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIExtensions 
{
    public static bool IsPointOverUI(this GraphicRaycaster raycaster, Vector2 point)
    {
        PointerEventData data = new(EventSystem.current)
        {
            position = point,
        };
        List<RaycastResult> results = new();
        raycaster.Raycast(data, results);
        return results.Count > 0;
    }
}
