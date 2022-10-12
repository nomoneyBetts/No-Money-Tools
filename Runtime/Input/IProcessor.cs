using System.Collections.Generic;

namespace NoMoney.InputSystem
{
    public interface IProcessor
    {
        void Process(List<InputData> inputs);
    }
}
