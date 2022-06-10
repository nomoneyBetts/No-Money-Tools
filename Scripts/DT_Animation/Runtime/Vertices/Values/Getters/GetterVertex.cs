namespace NoMoney.DTAnimation
{
    public abstract class GetterVertex<T> : ValueVertex<T>
    {
        public ExposedMethod ExposedMethod;

        public T GetValue()
        {
            if (ExposedMethod == null) return default;
            return ExposedMethod.Invoke<T>();
        }
    }
}
