using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney
{
    /// <summary>
    /// The visual element which displays the different kinds of allowed parameters.
    /// </summary>
    internal class MethodParameterElement : VisualElement, INotifyValueChanged<MethodParameter>
    {
        public MethodParameter value
        {
            get
            {
                MethodParameter value = null;
                if (childCount != 1) return value;
                switch (ElementAt(0))
                {
                    case IntegerField intField:
                        value = new MethodParameter
                        {
                            Name = intField.label,
                            ParameterType = typeof(int),
                            IntValue = intField.value
                        };
                        break;
                    case FloatField floatField:
                        value = new MethodParameter
                        {
                            Name = floatField.label,
                            ParameterType = typeof(float),
                            FloatValue = floatField.value
                        };
                        break;
                    case Toggle toggle:
                        value = new MethodParameter
                        {
                            Name = toggle.label,
                            ParameterType = typeof(bool),
                            BoolValue = toggle.value
                        };
                        break;
                    case TextField textField:
                        value = new MethodParameter
                        {
                            Name = textField.label,
                            ParameterType = typeof(string),
                            StringValue = textField.value
                        };
                        break;
                    case ObjectField objField:
                        value = new MethodParameter
                        {
                            Name = objField.label,
                            ParameterType = objField.objectType,
                            ObjectValue = objField.value
                        };
                        break;
                    case Vector2Field vec2Field:
                        value = new MethodParameter
                        {
                            Name = vec2Field.label,
                            ParameterType = typeof(Vector2),
                            VectorValue = vec2Field.value
                        };
                        break;
                    case Vector3Field Vec3Field:
                        value = new MethodParameter
                        {
                            Name = Vec3Field.label,
                            ParameterType = typeof(Vector3),
                            VectorValue = Vec3Field.value
                        };
                        break;
                    case Vector4Field vec4Field:
                        value = new MethodParameter
                        {
                            Name = vec4Field.label,
                            ParameterType = typeof(Vector4),
                            VectorValue = vec4Field.value
                        };
                        break;
                    case ColorField colorField:
                        value = new MethodParameter
                        {
                            Name = colorField.label,
                            ParameterType = typeof(Color),
                            VectorValue = colorField.value
                        };
                        break;
                    case CurveField curveField:
                        value = new MethodParameter
                        {
                            Name = curveField.label,
                            ParameterType = typeof(AnimationCurve),
                            CurveValue = curveField.value
                        };
                        break;
                }
                return value;
            }
            set
            {
                if(value == null || value.ParameterType == null)
                {
                    Clear();
                    return;
                }
                CreateField(value);
                switch (ElementAt(0))
                {
                    case IntegerField intField:
                        intField.value = value.IntValue;
                        break;
                    case FloatField floatField:
                        floatField.value = value.FloatValue;
                        break;
                    case Toggle toggle:
                        toggle.value = value.BoolValue;
                        break;
                    case TextField textField:
                        textField.value = value.StringValue;
                        break;
                    case ObjectField objField:
                        objField.value = value.ObjectValue;
                        break;
                    case Vector2Field vec2Field:
                        vec2Field.value = value.VectorValue;
                        break;
                    case Vector3Field vec3Field:
                        vec3Field.value = value.VectorValue;
                        break;
                    case Vector4Field vec4Field:
                        vec4Field.value = value.VectorValue;
                        break;
                    case ColorField colorField:
                        colorField.value = value.VectorValue;
                        break;
                    case CurveField curveField:
                        curveField.value = value.CurveValue;
                        break;
                }
            }
        }

        public void SetValueWithoutNotify(MethodParameter newValue)
        {
            Clear();
            if (newValue == null || newValue.ParameterType == null) return;
            if (newValue.ParameterType == typeof(int))
            {
                IntegerField intField = new IntegerField(newValue.Name);
                intField.SetValueWithoutNotify(newValue.IntValue);
                intField.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = intField.label,
                        ParameterType = typeof(int),
                        IntValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = intField.label,
                        ParameterType = typeof(int),
                        IntValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(intField);
            }
            else if (newValue.ParameterType == typeof(float))
            {
                FloatField floatField = new FloatField(newValue.Name);
                floatField.SetValueWithoutNotify(newValue.FloatValue);
                floatField.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = floatField.label,
                        ParameterType = typeof(float),
                        FloatValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = floatField.label,
                        ParameterType = typeof(float),
                        FloatValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(floatField);
            }
            else if (newValue.ParameterType == typeof(bool))
            {
                Toggle toggle = new Toggle(newValue.Name);
                toggle.SetValueWithoutNotify(newValue.BoolValue);
                toggle.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = toggle.label,
                        ParameterType = typeof(bool),
                        BoolValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = toggle.label,
                        ParameterType = typeof(bool),
                        BoolValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(toggle);
            }
            else if (newValue.ParameterType == typeof(string))
            {
                TextField textField = new TextField(newValue.Name);
                textField.SetValueWithoutNotify(newValue.StringValue);
                textField.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = textField.label,
                        ParameterType = typeof(string),
                        StringValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = textField.label,
                        ParameterType = typeof(string),
                        StringValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(textField);
            }
            else if(newValue.ParameterType == typeof(Vector2))
            {
                Vector2Field field = new Vector2Field(newValue.Name);
                field.SetValueWithoutNotify(newValue.VectorValue);
                field.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Vector2),
                        VectorValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Vector2),
                        VectorValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(field);
            }
            else if(newValue.ParameterType == typeof(Vector3))
            {
                Vector3Field field = new Vector3Field(newValue.Name);
                field.SetValueWithoutNotify(newValue.VectorValue);
                field.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Vector3),
                        VectorValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Vector4),
                        VectorValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(field);
            }
            else if(newValue.ParameterType == typeof(Vector4))
            {
                Vector4Field field = new Vector4Field(newValue.Name);
                field.SetValueWithoutNotify(newValue.VectorValue);
                field.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Vector4),
                        VectorValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Vector4),
                        VectorValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(field);
            }
            else if(newValue.ParameterType == typeof(Color))
            {
                ColorField field = new ColorField(newValue.Name);
                field.SetValueWithoutNotify(newValue.VectorValue);
                field.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Color),
                        VectorValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(Color),
                        VectorValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(field);
            }
            else if(newValue.ParameterType == typeof(AnimationCurve))
            {
                CurveField field = new CurveField(newValue.Name);
                field.SetValueWithoutNotify(newValue.CurveValue);
                field.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(AnimationCurve),
                        CurveValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = field.label,
                        ParameterType = typeof(AnimationCurve),
                        CurveValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(field);
            }
            else if (newValue.ParameterType.IsAssignableFrom(typeof(Object)))
            {
                ObjectField objField = new ObjectField(newValue.Name);
                objField.objectType = newValue.ParameterType;
                objField.SetValueWithoutNotify(newValue.ObjectValue);
                objField.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    MethodParameter previousValue = new MethodParameter
                    {
                        Name = objField.label,
                        ParameterType = objField.objectType,
                        ObjectValue = evt.previousValue
                    };
                    MethodParameter newValue = new MethodParameter
                    {
                        Name = objField.label,
                        ParameterType = objField.objectType,
                        ObjectValue = evt.newValue
                    };
                    CreateEvent(previousValue, newValue);
                });
                Add(objField);
            }
            else
            {
                throw new System.Exception("Unknown Type, unable to create field: " + newValue.ParameterType);
            }

            void CreateEvent(MethodParameter previousValue, MethodParameter newValue)
            {
                using ChangeEvent<MethodParameter> change = ChangeEvent<MethodParameter>.GetPooled(previousValue, newValue);
                change.target = this;
                SendEvent(change);
            }
        }

        private void CreateField(MethodParameter value)
        {
            if (childCount == 0)
            {
                Add(Create());
                return;
            }

            // Check that the field is correct type - then maybe make it
            switch (ElementAt(0))
            {
                case IntegerField intField:
                    if (value.ParameterType == typeof(int)) intField.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case FloatField floatfield:
                    if (value.ParameterType == typeof(float)) floatfield.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case Toggle toggle:
                    if (value.ParameterType == typeof(bool)) toggle.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case TextField textField:
                    if (value.ParameterType == typeof(string)) textField.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case ObjectField objField:
                    if (value.ParameterType.IsAssignableFrom(typeof(Object)))
                    {
                        objField.objectType = value.ParameterType;
                        objField.label = value.Name;
                    }
                    else
                    {
                        Clear();
                        Add(Create());
                    }
                    break;
                case Vector2Field vec2Field:
                    if (value.ParameterType == typeof(Vector2)) vec2Field.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case Vector3Field vec3Field:
                    if (value.ParameterType == typeof(Vector3)) vec3Field.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case Vector4Field vec4Field:
                    if (value.ParameterType == typeof(Vector4)) vec4Field.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case ColorField colorField:
                    if (value.ParameterType == typeof(Color)) colorField.label = value.Name;
                    else Clear(); Add(Create());
                    break;
                case CurveField curveField:
                    if (value.ParameterType == typeof(AnimationCurve)) curveField.label = value.Name;
                    else Clear(); Add(Create());
                    break;
            }
  
            VisualElement Create()
            {
                VisualElement field = null;
                if (value.ParameterType == typeof(int))
                {
                    field = new IntegerField(value.Name);
                }
                if(value.ParameterType == typeof(float))
                {
                    field = new FloatField(value.Name);
                }
                if(value.ParameterType == typeof(string))
                {
                    field = new TextField(value.Name);
                }
                if(value.ParameterType == typeof(bool))
                {
                    field = new Toggle(value.Name);
                }
                if (value.ParameterType.IsAssignableFrom(typeof(Object)))
                {
                    ObjectField objField = new ObjectField(value.Name);
                    objField.objectType = value.ParameterType;
                    field = objField;
                }
                if(value.ParameterType == typeof(Vector2))
                {
                    field = new Vector2Field(value.Name);
                }
                if(value.ParameterType == typeof(Vector3))
                {
                    field = new Vector3Field(value.Name);
                }
                if(value.ParameterType == typeof(Vector4))
                {
                    field = new Vector4Field(value.Name);
                }
                if(value.ParameterType == typeof(Color))
                {
                    field = new ColorField(value.Name);
                }
                if(value.ParameterType == typeof(AnimationCurve))
                {
                    field = new CurveField(value.Name);
                }
                return field;
            }
        }
    }
}
