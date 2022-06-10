using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NoMoney
{
    [Serializable]
    internal class MethodParameter
    {
        public string Name;
        [SerializeField]
        private string _typeString;
        public Type ParameterType
        {
            get => _typeString == null ? null : Type.GetType(_typeString);
            set => _typeString = value.AssemblyQualifiedName;
        }

        public bool IsVector
        {
            get => ParameterType == typeof(Vector2) || ParameterType == typeof(Vector3) || ParameterType == typeof(Vector4);
        }

        public static bool IsSupportedType(Type type)
        {
            return type == typeof(int) || type == typeof(float) || type == typeof(string) || type == typeof(bool) ||
                type == typeof(Object) || type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4) ||
                type == typeof(Color) || type == typeof(AnimationCurve);
        }

        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public string StringValue;
        public Object ObjectValue;
        public Vector4 VectorValue;
        public AnimationCurve CurveValue;
    }
}
