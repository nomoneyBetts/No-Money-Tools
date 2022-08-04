// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine.UIElements;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    public class CustomTweenNode : TweenNode
    {
        public CustomTweenNode(CustomTweenVertex vertex) : base(vertex)
        {
            title = "Custom Tween Node";
            MethodField field = new MethodField();
            field.ReturnType = typeof(Tween);
            field.SetValueWithoutNotify(vertex.ExposedMethod);
            field.RegisterValueChangedCallback(evt => vertex.ExposedMethod = evt.newValue);
            extensionContainer.Add(field);
            RefreshExpandedState();
        }
    }
}
