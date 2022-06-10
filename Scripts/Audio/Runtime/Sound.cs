using UnityEngine;
using UnityEngine.Audio;

namespace NoMoney.Audio
{
    internal class Sound : ScriptableObject
    {
        public string Name = "NAME ME";

        public AudioClip Clip;
        public AudioMixerGroup Output;

        public bool Mute;
        public bool BypassEffects;
        public bool BypassListenerEffects;
        public bool BypassReverbZone;
        public bool PlayOnAwake;
        public bool Loop;

        [Range(0, 256)]
        public int Priority = 128;
        [Range(0f, 1f)]
        public float Volume = 1f;
        [Range(-3f, 3f)]
        public float Pitch = 1f;
        [Range(-1f, 1f)]
        public float StereoPan = 0f;
        [Range(0f, 1f)]
        public float SpatialBlend = 0f;
        [Range(0f, 1.1f)]
        public float ReverbZoneMix = 1f;

        public void Apply(AudioSource source)
        {
            source.clip = Clip;
            source.outputAudioMixerGroup = Output;

            source.mute = Mute;
            source.bypassEffects = BypassEffects;
            source.bypassListenerEffects = BypassListenerEffects;
            source.bypassReverbZones = BypassReverbZone;
            source.playOnAwake = PlayOnAwake;
            source.loop = Loop;

            source.priority = Priority;
            source.volume = Volume;
            source.pitch = Pitch;
            source.panStereo = StereoPan;
            source.spatialBlend = SpatialBlend;
            source.reverbZoneMix = ReverbZoneMix;
        }

#if UNITY_EDITOR
        public void DestroySound() => DestroyImmediate(this);
#endif
    }
}
