using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recording
{
    public class RecordManager : MonoBehaviour
    {
        [SerializeField]
        private float _globalMaxRecordingTime = 10f;
        public float GlobalMaxRecordingTime { get => _globalMaxRecordingTime; }
        [SerializeField]
        [Range(0f, 5f)]
        private float _globalTrackSpeed = 1f;
        public float GlobalTrackSpeed { get => _globalTrackSpeed; }
        [SerializeField]
        private bool _defaultOverwriteWhenFull;

        private readonly Dictionary<string, Track> _tracks = new();

        private void Update()
        {
            foreach(Track track in _tracks.Values)
            {
                track.Update();
            }
        }

        #region CRUD
        public void CreateTransformTrack(string name) => CreateTrack<TransformFrame>(name);
        public Track CreateTrack<T>(string name) where T : Frame => CreateTrack<T>(name, gameObject);
        public Track CreateTrack<T>(string name, GameObject target) where T : Frame
        {
            Track track;
            if (_tracks.ContainsKey(name))
            {
                track = _tracks[name];
                track.ClearTrack();
                track.MaxRecordingTime = GlobalMaxRecordingTime;
                track.OverwriteWhenFull = _defaultOverwriteWhenFull;
            }
            else
            {
                track = new Track(this, target, typeof(T))
                {
                    OverwriteWhenFull = _defaultOverwriteWhenFull
                };
                _tracks.Add(name, track);
            }
            return track;
        }

        public void Delete(string name)
        {
            Track track = _tracks[name];
            track.TryChange(Track.Stop);
            track.ClearTrack();
            _tracks.Remove(name);
        }

        public void Record(string name) => RecordTrack(name);
        public Track RecordTrack(string name) => StateChange(name, Track.Record);
        #endregion

        #region Interact
        public void Play(string name) => PlayTrack(name);
        public Track PlayTrack(string name) => StateChange(name, Track.Play);

        public void Pause(string name) => PauseTrack(name);
        public Track PauseTrack(string name) => StateChange(name, Track.Pause);
        public void TogglePause(string name) => _tracks[name].TogglePause();

        public void Rewind(string name) => RewindTrack(name);
        public Track RewindTrack(string name) => StateChange(name, Track.Rewind);

        public void Stop(string name) => StopTrack(name);
        public Track StopTrack(string name) => StateChange(name, Track.Stop);

        public Track GetTrack(string name) => _tracks[name];
        #endregion
        
        private Track StateChange(string name, string newState)
        {
            Track track = _tracks[name];
            if (!track.TryChange(newState))
            {
                Debug.LogError($"Failed to change track state: {track.State.GetType()} -> {newState}");
            }
            return track;
        }
    }
}
