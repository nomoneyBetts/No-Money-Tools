namespace StateMachine
{
    public abstract class State
    {
        public readonly IStateMachine Machine;

        public State(IStateMachine machine)
        {
            Machine = machine;
        }

        public abstract State Run(object[] parameters = null);
        public abstract bool CanChangeState(string state);
        public abstract void ApplyState();
    }
}
