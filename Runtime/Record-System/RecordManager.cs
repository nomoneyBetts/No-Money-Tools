using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.RecordSystem
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
        /// <summary>
        /// Creates a Track with the given name and targets the GameObject this manager is attached to.
        /// </summary>
        /// <typeparam name="T">The type of Frame the Track will record.</typeparam>
        /// <param name="name">The name of the Track.</param>
        /// <returns>The created Track.</returns>
        public Track CreateTrack<T>(string name) where T : Frame => CreateTrack<T>(name, gameObject);
        /// <summary>
        /// Create a new Track with the given name and an object to target for recording.
        /// </summary>
        /// <typeparam name="T">The type of Frame the track will record.</typeparam>
        /// <param name="name">The name of the Track.</param>
        /// <param name="target">The GameObject to record.</param>
        /// <returns>The created track.</returns>
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

        /// <summary>
        /// Deletes a Track from this manager's dictionary.
        /// </summary>
        /// <param name="name">The name of the Track to delete.</param>
        public void Delete(string name)
        {
            Track track = _tracks[name];
            track.ChangeState(Track.Stop);
            track.ClearTrack();
            _tracks.Remove(name);
        }
        #endregion

        #region Interact
        /// <summary>
        /// Begin recording a Track.
        /// </summary>
        /// <param name="name">The name of the Track to record.</param>
        public void Record(string name) => RecordTrack(name);
        /// <summary>
        /// Begin recording a Track.
        /// </summary>
        /// <param name="name">The name of the Track to record.</param>
        /// <returns>The recorded Track.</returns>
        public Track RecordTrack(string name) => StateChange(name, Track.Record);

        /// <summary>
        /// Begin playing a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        public void Play(string name) => PlayTrack(name);
        /// <summary>
        /// Begin playing a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        /// <returns>The playing track.</returns>
        public Track PlayTrack(string name) => StateChange(name, Track.Play);

        /// <summary>
        /// Pause a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        public void Pause(string name) => PauseTrack(name);
        /// <summary>
        /// Pause a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        /// <returns>The paused Track.</returns>
        public Track PauseTrack(string name) => StateChange(name, Track.Pause);
        /// <summary>
        /// If the Track was playing or rewinding then it is now paused.
        /// If the Track was paused, then it returns to its previous state.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        public void TogglePause(string name) => _tracks[name].TogglePause();

        /// <summary>
        /// Rewind a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        public void Rewind(string name) => RewindTrack(name);
        /// <summary>
        /// Rewind a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        /// <returns>The rewinding Track.</returns>
        public Track RewindTrack(string name) => StateChange(name, Track.Rewind);

        /// <summary>
        /// Stop a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        public void Stop(string name) => StopTrack(name);
        /// <summary>
        /// Stop a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        /// <returns>The stopped Track.</returns>
        public Track StopTrack(string name) => StateChange(name, Track.Stop);

        /// <summary>
        /// Get a Track.
        /// </summary>
        /// <param name="name">The name of the Track.</param>
        public Track GetTrack(string name) => _tracks[name];
        #endregion
        
        private Track StateChange(string name, string newState)
        {
            Track track = _tracks[name];
            if (!track.TryChangeState(newState))
            {
                Debug.LogError($"Failed to change track state: {track.State.GetType()} -> {newState}");
            }
            return track;
        }
    }
}
