using StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Recording.TrackState
{
    public class RewindState : State
    {
        public RewindState(Track track) : base(track)
        {
            ApplyState();
        }

        public override void ApplyState()
        {
            Track track = (Track)Machine;
            int end = track.CurFrame == null ? 1 : 0;
            track.ApplyFrame(end);
            if(end > 0)
            {
                track.RewindStart?.Invoke();
            }
            track.EnterRewind?.Invoke();
        }

        public override bool CanChangeState(string state) => 
            state == Track.Stop || state == Track.Play || state == Track.Pause;

        public override State Run(object[] parameters = null)
        {
            Track track = (Track)Machine;
            float deltaTime = Time.deltaTime * track.TrackSpeed;

            // Playback time may be different from recording, so find the best frame.
            LinkedListNode<Frame> prev;
            LinkedListNode<Frame> current = track.CurFrame;
            // Stop if the current frame is null.
            if (current == null) return new StopState(track);

            while ((prev = current.Previous) != null)
            {
                float frameDelta = current.Value.Timestamp - prev.Value.Timestamp;
                float curtime = current.Value.Timestamp + current.Value.Interpolation * frameDelta;
                if (curtime - deltaTime > prev.Value.Timestamp)
                {
                    current.Value.Interpolation = -(1 - (curtime - deltaTime - prev.Value.Timestamp) / frameDelta);
                    break;
                }

                // Switch frames and keep checking
                current.Value.Interpolation = 0f;
                deltaTime -= frameDelta;
                current = prev;
            }
            track.CurFrame = current;
            track.ApplyFrame();

            // If the frame is on the end, switch to stop, otherwise stay in rewind
            if (track.IsFirst())
            {
                track.RewindComplete?.Invoke();
                return new StopState(track);
            }
            return this;
        }
    }
}
