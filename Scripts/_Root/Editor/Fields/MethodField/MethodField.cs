using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

namespace NoMoney
{
    /// <summary>
    /// A field for inputing ExposedMethods.
    /// </summary>
    public class MethodField : NMBaseField<ExposedMethod>
    {
        // If return type is set, only allow methods that return the type.
        private Type _returnType;
        public Type ReturnType
        {
            get => _returnType;
            set
            {
                _returnType = value;
                UpdateSelector();
            }
        }

        // If parameter types is set, only allow methods that have parameters
        // in the order of the the types.
        private Type[] _parameterTypes;
        public Type[] ParameterTypes
        {
            get => _parameterTypes;
            set
            {
                _parameterTypes = value;
                UpdateSelector();
            }
        }

        public override ExposedMethod value
        {
            get
            {
                string method = this.Q<ToolbarMenu>(Selector).text.Split('\t')[0];
                return new ExposedMethod
                {
                    Target = this.Q<ObjectField>(Target).value,
                    Method = method == "UNSET" ? null : method,
                    Parameters = this.Q<MethodParameterListField>(Parameters).value
                };
            }
            set
            {
                ObjectField targetField = this.Q<ObjectField>(Target);
                MethodParameterListField parameters = this.Q<MethodParameterListField>(Parameters);

                if (value == null)
                {
                    targetField.value = null;
                    UpdateSelector();
                    parameters.value = null;
                }
                else
                {
                    targetField.value = value.Target;
                    UpdateSelector(value.Method);
                    parameters.value = value.Parameters;
                }
            }
        }

        private const string Target = "target";
        private const string Selector = "selector";
        private const string Parameters = "parameters";

        private readonly Dictionary<Type, Color> _typeColors = new Dictionary<Type, Color>()
        {
            { typeof(int), new Color(148f / 255, 129f / 255, 230f / 255) },
            { typeof(float), new Color(132f / 255, 228f / 255, 231f / 255) },
            { typeof(string), new Color(252f / 255, 215f / 255, 110f / 255) },
            { typeof(Color), new Color(251f / 255, 203f / 255, 244f / 255) },
            { typeof(Vector2), new Color(154f / 255, 239f / 255, 146f / 255) },
            { typeof(Vector3), new Color(201f / 255, 247f / 255, 116f / 255) },
            { typeof(Vector4), new Color(251f / 255, 203f / 255, 244f / 255) }
        };
        private readonly Color _defaultColor = new Color(200f / 255, 200f / 255, 200f / 255);

        public MethodField()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("MethodField");
            visualTree.CloneTree(this);
            styleSheets.Add(Resources.Load<StyleSheet>("MethodField"));

            // Register Target Field change
            this.Q<ObjectField>(Target).RegisterValueChangedCallback(evt =>
            {
                evt.StopPropagation();
                ExposedMethod previousValue = value;
                previousValue.Target = evt.previousValue;
                UpdateSelector();
                this.Q<MethodParameterListField>(Parameters).SetValueWithoutNotify(null);
                ExposedMethod newValue = value;
                newValue.Target = evt.newValue;

                using ChangeEvent<ExposedMethod> change = ChangeEvent<ExposedMethod>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            });

            // Register Parameters change
            this.Q<MethodParameterListField>(Parameters).RegisterValueChangedCallback(evt =>
            {
                evt.StopPropagation();
                ExposedMethod previousValue = value;
                previousValue.Parameters = evt.previousValue;
                ExposedMethod newValue = value;
                newValue.Parameters = evt.newValue;

                using ChangeEvent<ExposedMethod> change = ChangeEvent<ExposedMethod>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            });
        }

        public MethodField(string title) : this() => Title = title;

        public override void SetValueWithoutNotify(ExposedMethod newValue)
        {
            ObjectField targetField = this.Q<ObjectField>(Target);
            MethodParameterListField parameters = this.Q<MethodParameterListField>(Parameters);

            if(newValue == null)
            {
                targetField.SetValueWithoutNotify(null);
                UpdateSelector();
                parameters.SetValueWithoutNotify(null);
            }
            else
            {
                targetField.SetValueWithoutNotify(newValue.Target);
                UpdateSelector(newValue.Method);
                parameters.SetValueWithoutNotify(newValue.Parameters);
            }
        }

        /// <summary>
        /// Updates the selector text and repopulates the menu.
        /// </summary>
        /// <param name="text"></param>
        private void UpdateSelector(string text = "UNSET")
        {
            ToolbarMenu selector = this.Q<ToolbarMenu>(Selector);
            Object target = this.Q<ObjectField>(Target).value;

            // Extract method name and determine return type
            text = string.IsNullOrEmpty(text) ? "UNSET" : text.Split('\t')[0];
            MethodInfo methodInfo = target == null ? null : target.GetMethod(text);
            Type currentReturnType = text == "UNSET" || target == null || methodInfo == null ?
                null : methodInfo.ReturnType;
            if (currentReturnType == null && ReturnType != null) currentReturnType = ReturnType;

            // Set text and style color based on return type
            selector.text = text;
            selector.style.color = (text == "UNSET" && ReturnType == null) || 
                currentReturnType == null || !_typeColors.ContainsKey(currentReturnType) ?
                _defaultColor : 
                _typeColors[currentReturnType];
            if (currentReturnType != null && currentReturnType != typeof(void)) selector.text += $"\t|\t{currentReturnType.FullName}";

            // The UNSET action
            selector.menu.AppendAction("UNSET", a =>
            {
                ExposedMethod previousValue = value;
                selector.text = "UNSET";
                if (ReturnType != null && ReturnType != typeof(void)) selector.text += $"\t|\t{ReturnType.FullName}";
                this.Q<MethodParameterListField>(Parameters).SetValueWithoutNotify(null);
                ExposedMethod newValue = value;

                using ChangeEvent<ExposedMethod> change = ChangeEvent<ExposedMethod>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            });

            // Populate the selector
            if (target == null) return;
            if (target is GameObject goTarget)
            {
                Populate("GameObject", target);
                foreach (Component component in goTarget.GetComponents<MonoBehaviour>())
                {
                    Populate(component.GetType().Name, component);
                }
            }
            else
            {
                Populate(null, target);
            }

            // Helper Methods
            void Populate(string root, Object target)
            {
                foreach (MethodInfo info in target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if ((ReturnType != null && info.ReturnType != ReturnType) || !ParametersSupported(info.GetParameters())) continue;

                    string name = string.IsNullOrEmpty(root) ? info.Name : $"{root}/{info.Name}";
                    selector.menu.AppendAction(name, a =>
                    {
                        selector.style.color = _typeColors.ContainsKey(info.ReturnType) ?
                            _typeColors[info.ReturnType] : _defaultColor;

                        ExposedMethod previousValue = value;
                        selector.text = info.ReturnType == typeof(void) ?
                            info.Name : $"{info.Name}\t|\t{info.ReturnType}";

                        ParameterInfo[] pInfos = info.GetParameters();
                        List<MethodParameter> methodParams = new List<MethodParameter>(pInfos.Length);
                        foreach (ParameterInfo pInfo in pInfos)
                        {
                            methodParams.Add(new MethodParameter
                            {
                                Name = pInfo.Name,
                                ParameterType = pInfo.ParameterType
                            });
                        }
                        this.Q<MethodParameterListField>(Parameters).SetValueWithoutNotify(methodParams);

                        ExposedMethod newValue = value;

                        using ChangeEvent<ExposedMethod> change = ChangeEvent<ExposedMethod>.GetPooled(previousValue, newValue);
                        change.target = this;
                        SendEvent(change);
                    });
                }
            }

            bool ParametersSupported(ParameterInfo[] parameters)
            {
                if (ParameterTypes != null && ParameterTypes.Length != parameters.Length) return false;
                for(int i = 0; i < parameters.Length; i++)
                {
                    if (!MethodParameter.IsSupportedType(parameters[i].ParameterType)) return false;
                    if (ParameterTypes != null && ParameterTypes[i] != parameters[i].ParameterType) return false;
                }
                return true;
            }
        }

        #region UXML Factory
        public new class UxmlFactory : UxmlFactory<MethodField, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _title =
                new UxmlStringAttributeDescription { name = "title", defaultValue = null };
            private readonly UxmlTypeAttributeDescription<object> _returnType =
                new UxmlTypeAttributeDescription<object> { name = "return-type", defaultValue = null };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext ctx)
            {
                base.Init(element, bag, ctx);
                MethodField field = (MethodField)element;

                field.Title = _title.GetValueFromBag(bag, ctx);
                field.ReturnType = _returnType.GetValueFromBag(bag, ctx);
            }
        }
        #endregion
    }
}
