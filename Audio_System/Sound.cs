using UnityEngine;
using UnityEngine.Audio;

namespace Audio_System
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Audio System")]
    public class Sound : ScriptableObject
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        
        public bool loop, mute, byPassEffects, byPassListenerEffects, 
            byPassReverbZones, playOnAwake;

        [Range(0, 256)]
        public int priority = 128;

        [Range(0, 1)]
        public float volume = 1;

        [Range(-3, 3)]
        public float pitch = 1;

        [Range(-1, 1)]
        public float stereoPan = 0;

        [Range(0, 1)]
        public float spatialBlend = 0;

        [Range(0, 1.1f)]
        public float reverbZoneMix = 1;
    }
}
