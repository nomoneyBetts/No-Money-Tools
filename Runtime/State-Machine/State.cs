namespace NoMoney.StateMachine
{
    public abstract class State
    {
        public readonly IStateMachine Machine;

        public State(IStateMachine machine)
        {
            Machine = machine;
        }

        /// <summary>
        /// Call this when the state should be applied - or reapplied.
        /// Good practice is to also call this from the constructor.
        /// </summary>
        public abstract void ApplyState();

        /// <param name="state">The name of the state you wish to change to.</param>
        /// <returns>True if this state can change to the param state.</returns>
        public abstract bool CanChangeState(string state);

        /// <summary>
        /// Run the state.
        /// This is usually called from the State Machine's Update method.
        /// </summary>
        /// <param name="parameters">Any additional parameters the state needs to run.</param>
        /// <returns>An updated State. Usually return "this" if there were no changes.</returns>
        public abstract State Run(object[] parameters = null);
    }
}
