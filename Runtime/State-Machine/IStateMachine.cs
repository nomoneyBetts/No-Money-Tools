namespace StateMachine
{
    public interface IStateMachine
    {
        State State { get; }
        bool TryChange(string state);
    }
}
