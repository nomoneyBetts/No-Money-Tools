using UnityEngine;

namespace Audio_System
{
    /// <summary>
    /// The sound, song, noise, etc. to play.
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        public string tag;

        [Header("Attached Source")]
        //either add a source yourself
        public AudioSource source;

        [Header("Detached Source")]
        //or make one
        public AudioClip clip;
        [Range(0, 256)]
        public int priority = 128;
        [Range(0f, 1f)]
        public float volume = 1f, spatialBlend = 0f;
        [Range(-3f, 3f)]
        public float pitch = 1f;
        [Range(-1f, 1f)]
        public float stereoPan = 0f;
        [Range(0f, 1.1f)]
        public float reverbZoneMix = 1f;

        public bool mute, bypassEffects, bypassListenerEffects, bypassReverbZones, playOnAwake, loop;
    }
}