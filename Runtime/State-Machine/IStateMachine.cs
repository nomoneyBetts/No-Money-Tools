namespace NoMoney.StateMachine
{
    public interface IStateMachine
    {
        State State { get; }
        void ChangeState(string state);
    }
}
