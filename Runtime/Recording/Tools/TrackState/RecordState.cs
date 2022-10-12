using StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recording.TrackState
{
    public class RecordState : State
    {
        private readonly LinkedList<Frame> _frames;
        private readonly GameObject _target;

        public RecordState(Track track, LinkedList<Frame> frames, GameObject target) : base(track)
        {
            _frames = frames;
            _target = target;
            ApplyState();
        }

        public override void ApplyState()
        {
            Track track = (Track)Machine;
            track.ClearTrack();
            RecordFrame(track);
            track.EnterRecord?.Invoke();
        }

        public override bool CanChangeState(string state) => state == Track.Stop;

        public override State Run(object[] parameters = null)
        {
            Track track = (Track)Machine;
            bool full = track.TrackLength > track.MaxRecordingTime || track.TrackLength > track.GlobalMaxRecordingTime;
            if (full && track.OverwriteWhenFull) _frames.RemoveFirst();
            RecordFrame(track);
            return full && !track.OverwriteWhenFull ? new StopState(track) : this;
        }

        private void RecordFrame(Track track)
        {
            Frame frame = (Frame)Activator.CreateInstance(track.FrameType, new object[] { track.TrackLength });
            frame.RecordData(_target);

            _frames.AddLast(frame);
            track.TrackLength += Time.deltaTime;
        }
    }
}
