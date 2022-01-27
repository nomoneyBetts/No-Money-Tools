using System.Collections.Generic;
using UnityEngine;
using NoMoney_Core;

namespace Audio_System
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private string soundPaths;
        public List<Sound> tempSounds;
        private List<AudioSource> activeSources;
        private Dictionary<string, Sound> sounds;

        private void Awake()
        {
            activeSources = new List<AudioSource>();
            sounds = new Dictionary<string, Sound>();
            foreach(Sound s in tempSounds)
            {
                if (s != null)
                {
                    sounds.Add(s.name, s);
                }
            }
        }

        /// <summary>
        /// Plays a new sound for the given source.
        /// </summary>
        /// <param name="name">The name of the sound to play.</param>
        /// <param name="source">The audio source to play the sound on.</param>
        public void PlaySound(string name, AudioSource source)
        {
            source.Stop();
            ApplySound(sounds[name], source);
            source.Play();
            if (!activeSources.Contains(source))
            {
                activeSources.Add(source);
            }
        }

        /// <summary>
        /// Pauses or plays the source.
        /// </summary>
        /// <param name="source">The audio source to pause or play.</param>
        public void TogglePause(AudioSource source)
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
            else
            {
                source.Play();
            }
        }

        /// <summary>
        /// Applies the sound to the source.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="source"></param>
        private void ApplySound(Sound sound, AudioSource source)
        {
            source.clip = sound.clip;
            source.outputAudioMixerGroup = sound.mixerGroup;

            source.loop = sound.loop;
            source.mute = sound.mute;
            source.bypassEffects = sound.byPassEffects;
            source.bypassReverbZones = sound.byPassReverbZones;
            source.playOnAwake = sound.playOnAwake;

            source.priority = sound.priority;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.panStereo = sound.stereoPan;
            source.spatialBlend = sound.spatialBlend;
            source.reverbZoneMix = sound.reverbZoneMix;
        }
    }

}
