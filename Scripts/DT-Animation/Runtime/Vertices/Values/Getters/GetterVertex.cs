// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

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
