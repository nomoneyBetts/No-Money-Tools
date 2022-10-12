using StateMachine;

namespace Recording.TrackState
{
    public class PauseState : State
    {
        private readonly State _prevState;

        public PauseState(IStateMachine machine, State prevState) : base(machine)
        {
            _prevState = prevState;
            ApplyState();
        }

        public override void ApplyState()
        {
            ((Track)Machine).EnterPause?.Invoke();
        }

        public override bool CanChangeState(string state) => 
            state == Track.Play || state == Track.Rewind || state == Track.Stop;

        public override State Run(object[] parameters = null)
        {
            ((Track)Machine).ApplyFrame();
            return this;
        }

        public State Resume() => _prevState;
    }
}
