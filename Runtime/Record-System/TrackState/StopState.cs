using NoMoney.StateMachine;

namespace NoMoney.RecordSystem.TrackState
{
    public class StopState : State
    {
        public StopState(Track track) : base(track)
        {
            ApplyState();
        }

        /// <summary>
        /// Reset track's current frame.
        /// </summary>
        public override void ApplyState()
        {
            Track track = (Track)Machine;
            track.CurFrame = null;
            track.EnterStop?.Invoke();
        }

        public override bool CanChangeState(string state) => 
            state == Track.Record || state == Track.Stop || state == Track.Play || state == Track.Rewind;

        /// <summary>
        /// Do nothing;
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override State Run(object[] parameters = null) => this;
    }
}
