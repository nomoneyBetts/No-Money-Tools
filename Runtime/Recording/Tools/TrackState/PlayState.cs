using StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Recording.TrackState
{
    public class PlayState : State
    {
        public PlayState(Track track) : base(track)
        {
            ApplyState();
        }

        public override void ApplyState()
        {
            Track track = (Track)Machine;
            int end = track.CurFrame == null ? -1 : 0;
            track.ApplyFrame(end);
            if(end < 0)
            {
                track.PlayStart?.Invoke();
            }
            track.EnterPlay?.Invoke();
        }

        public override bool CanChangeState(string state) =>
            state == Track.Stop || state == Track.Rewind || state == Track.Pause;

        public override State Run(object[] parameters = null)
        {
            Track track = (Track)Machine;
            float deltaTime = Time.deltaTime * track.TrackSpeed;

            // Playback time may be different from recording, so find the best frame.
            LinkedListNode<Frame> next;
            LinkedListNode<Frame> current = track.CurFrame;
            // Stop if the current frame is null.
            if (current == null) return new StopState(track);

            while((next = current.Next) != null)
            {
                float frameDelta = next.Value.Timestamp - current.Value.Timestamp;
                float curtime = current.Value.Timestamp + current.Value.Interpolation * frameDelta;
                if (curtime + deltaTime < next.Value.Timestamp)
                {
                    current.Value.Interpolation = (curtime + deltaTime - current.Value.Timestamp) / frameDelta;
                    break;
                }

                // Switch frames and keep checking
                current.Value.Interpolation = 0f;
                deltaTime -= frameDelta;
                current = next;
            }
            track.CurFrame = current;
            track.ApplyFrame();

            // If the frame is on the end, switch to stop, otherwise stay in play
            if (track.IsLast())
            {
                track.PlayComplete?.Invoke();
                return new StopState(track);
            }
            return this;
        }
    }
}
