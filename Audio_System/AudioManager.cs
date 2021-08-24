using System.Collections.Generic;
using UnityEngine;

namespace Audio_System
{
    /// <summary>
    /// This class holds a list of AudioClips that are applied to 
    /// AudioSources that are created during run time
    /// Every Object that is capable of making a sound should have an AudioManager
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        //Use this for theme music or something that should transition between scenes.
        [SerializeField]
        private bool dontDestroyOnLoad;
        public static AudioManager Instance;

        [SerializeField]
        private Sound[] sounds;
        public Dictionary<string, AudioSource> sources { get; private set; }

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                if (Instance != null)
                {
                    Destroy(this);
                }
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(this);
                }
            }

            if (sounds == null) return;
            sources = new Dictionary<string, AudioSource>();
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].source != null)
                {
                    //This audio source must already attached to the object, just add it to the dictionary
                    sources.Add(sounds[i].tag, sounds[i].source);
                }
                else
                {
                    //make and audio source
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.clip = sounds[i].clip;

                    //set the bools
                    source.mute = sounds[i].mute;
                    source.bypassEffects = sounds[i].bypassEffects;
                    source.bypassListenerEffects = sounds[i].bypassListenerEffects;
                    source.bypassReverbZones = sounds[i].bypassReverbZones;
                    source.playOnAwake = sounds[i].playOnAwake;
                    source.loop = sounds[i].loop;

                    //set the floats
                    source.priority = sounds[i].priority;
                    source.volume = sounds[i].volume;
                    source.spatialBlend = sounds[i].spatialBlend;
                    source.pitch = sounds[i].pitch;
                    source.panStereo = sounds[i].stereoPan;
                    source.reverbZoneMix = sounds[i].reverbZoneMix;

                    //add to the book
                    sources.Add(sounds[i].tag, source);
                }
            }
        }

        public static System.Exception soundNotFound =
            new System.Exception("This Tag is Not Recognized");
        private void TestContains(string tag)
        {
            if (!sources.ContainsKey(tag))
                throw soundNotFound;
        }

        /// <summary>
        /// Plays the sound relating to the tag.
        /// </summary>
        /// <param name="tag">
        /// The name of the sound to play.
        /// </param>
        public void PlaySound(string tag)
        {
            TestContains(tag);
            sources[tag].Play();
        }

        /// <summary>
        /// Stops the sound relating to the tag.
        /// </summary>
        /// <param name="tag">
        /// The name of the sound to stop.
        /// </param>
        public void StopSound(string tag)
        {
            TestContains(tag);
            sources[tag].Stop();
        }

        /// <summary>
        /// Pauses the sound relating to the tag.
        /// </summary>
        /// <param name="tag">
        /// The name of the sound to pause.
        /// </param>
        public void PauseSound(string tag)
        {
            TestContains(tag);
            sources[tag].Pause();
        }

        /// <summary>
        /// UnPauses the sound relating to the tag.
        /// </summary>
        /// <param name="tag">
        /// The name of the sound to UnPause.
        /// </param>
        public void UnPauseSound(string tag)
        {
            TestContains(tag);
            sources[tag].UnPause();
        }

        /// <summary>
        /// Is the sound currently playing?
        /// </summary>
        /// <param name="tag">
        /// The name of the sound check.
        /// </param>
        /// <returns>
        /// If the sound is playing or not.
        /// </returns>
        public bool IsPlaying(string tag)
        {
            TestContains(tag);
            return sources[tag].isPlaying;
        }
    }
}