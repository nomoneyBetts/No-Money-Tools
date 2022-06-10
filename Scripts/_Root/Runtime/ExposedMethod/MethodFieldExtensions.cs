using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NoMoney
{
    public static class MethodFieldExtensions
    {
        public static MethodInfo GetMethod(this Object target, string method)
        {
            Type type = target.GetType();
            return type.GetMethod(method);
        }

        internal static object[] ToObjArray(this List<MethodParameter> parameters)
        {
            object[] obj = new object[parameters.Count];
            int index = 0;
            foreach(MethodParameter parameter in parameters)
            {
                if(parameter.ParameterType == typeof(int))
                {
                    obj[index++] = parameter.IntValue;
                }
                else if(parameter.ParameterType == typeof(bool))
                {
                    obj[index++] = parameter.BoolValue;
                }
                else if(parameter.ParameterType == typeof(float))
                {
                    obj[index++] = parameter.FloatValue;
                }
                else if(parameter.ParameterType == typeof(string))
                {
                    obj[index++] = parameter.StringValue;
                }
                else if(parameter.IsVector || parameter.ParameterType == typeof(Color))
                {
                    obj[index++] = parameter.VectorValue;
                }
                else if(parameter.ParameterType == typeof(AnimationCurve))
                {
                    obj[index++] = parameter.CurveValue;
                }
                else if (parameter.ParameterType.IsAssignableFrom(typeof(Object)))
                {
                    obj[index++] = parameter.ObjectValue;
                }
            }
            return obj;
        }
    }
}
