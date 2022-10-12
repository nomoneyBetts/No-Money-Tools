using NoMoney.RecordSystem.TrackState;
using NoMoney.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.RecordSystem
{
    public class Track : IStateMachine
    {
        #region States
        private const string _record = "Record";
        private const string _play = "Play";
        private const string _stop = "Stop";
        private const string _pause = "Pause";
        private const string _rewind = "Rewind";
        public static string Record { get => _record; }
        public static string Stop { get => _stop; }
        public static string Play { get => _play; }
        public static string Pause { get => _pause; }
        public static string Rewind { get => _rewind; }
        #endregion

        public State State { get; private set; }
        public LinkedListNode<Frame> CurFrame = null;
        public float TrackSpeedMultiplier = 1f;
        public float TrackSpeed { get => _manager.GlobalTrackSpeed * TrackSpeedMultiplier; }
        public float TrackLength;
        public bool OverwriteWhenFull;
        public float MaxRecordingTime = float.MaxValue;
        public float GlobalMaxRecordingTime { get => _manager.GlobalMaxRecordingTime; }

        #region Callbacks
        public Action RewindComplete { get; private set; }
        public Action PlayComplete { get; private set; }
        public Action RewindStart { get; private set; }
        public Action PlayStart { get; private set; }
        public Action EnterPause { get; private set; }
        public Action EnterPlay { get; private set; }
        public Action EnterRewind { get; private set; }
        public Action EnterStop { get; private set; }
        public Action EnterRecord { get; private set; }
        #endregion

        public readonly Type FrameType;
        private readonly LinkedList<Frame> _frames = new();
        private readonly GameObject _target;
        private readonly RecordManager _manager;

        public Track(RecordManager manager, GameObject target, Type frameType)
        {
            _target = target;
            State = new StopState(this);
            _manager = manager;
            FrameType = frameType;
        }

        public void Update()
        {
            State = State.Run();
        }

        public void ChangeState(string state)
        {
            if (!State.CanChangeState(state)) return;

            State = state switch
            {
                _play => new PlayState(this),
                _pause => new PauseState(this, State),
                _rewind => new RewindState(this),
                _record => new RecordState(this, _frames, _target),
                _ => new StopState(this)
            };
        }

        public bool TryChangeState(string state)
        {
            if (!State.CanChangeState(state)) return false;
            ChangeState(state);
            return true;
        }

        /// <summary>
        /// If the track is in Play or Rewind, then it pauses.
        /// If the track is Paused, it resumes the last state it was in.
        /// </summary>
        public void TogglePause()
        {
            if (State is PauseState pauseState)
            {
                State = pauseState.Resume();
            }
            else if(!State.CanChangeState(Pause))
            {
                State = new PauseState(this, State);
            }
        }

        /// <summary>
        /// Apply the current frame.
        /// </summary>
        /// <param name="fromEnd">negative means start, positive means end, 0 is from where it currently is.</param>
        public void ApplyFrame(int fromEnd = 0)
        {
            if (fromEnd != 0)
            {
                CurFrame = fromEnd < 0 ? _frames.First : _frames.Last;
            }
            if (CurFrame == null) return;

            Frame frame = CurFrame.Value;
            Frame next = null;
            if (frame.Interpolation != 0f)
            {
                next = frame.Interpolation < 0 ? CurFrame.Previous.Value : CurFrame.Next.Value;
            }
            frame.Interpolate(_target, next);
        }

        /// <returns>True if the current frame is the last frame.</returns>
        public bool IsLast() => CurFrame != null && _frames.Last == CurFrame;

        /// <returns>True if the current frame is the first frame.</returns>
        public bool IsFirst() => CurFrame != null && _frames.First == CurFrame;

        /// <summary>
        /// Resets the track.
        /// </summary>
        public void ClearTrack()
        {
            _frames.Clear();
            CurFrame = null;
            TrackLength = 0f;
        }

        #region Callback Assignment
        public Track OnRewindComplete(Action onComplete)
        {
            RewindComplete = onComplete;
            return this;
        }

        public Track OnPlayComplete(Action onComplete)
        {
            PlayComplete = onComplete;
            return this;
        }

        public Track OnRewindStart(Action onStart)
        {
            RewindStart = onStart;
            return this;
        }

        public Track OnPlayStart(Action onStart)
        {
            PlayStart = onStart;
            return this;
        }

        public Track OnEnterPause(Action onPause)
        {
            EnterPause = onPause;
            return this;
        }

        public Track OnEnterPlay(Action onPlay)
        {
            EnterPlay = onPlay;
            return this;
        }

        public Track OnEnterRewind(Action onRewind)
        {
            EnterRewind = onRewind;
            return this;
        }

        public Track OnEnterStop(Action onStop)
        {
            EnterStop = onStop;
            return this;
        }

        public Track OnEnterRecord(Action onRecord)
        {
            EnterRecord = onRecord;
            return this;
        }
        #endregion
    }
}
