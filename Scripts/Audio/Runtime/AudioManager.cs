using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.Audio
{
    [System.Serializable]
    internal class StringSoundDict : SerializableDictionary<string, Sound> { }

    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private StringSoundDict _sounds;
        [SerializeField]
        private Sound _selectedSound;
        [SerializeField]
        private List<string> _loadedSounds;

        public void PlaySound(AudioSource source, string sound)
        {
            if (ErrorCheck(sound)) return;
            _sounds[sound].Apply(source);
            source.Play();
        }

        private bool ErrorCheck(string sound)
        {
            if(_sounds == null)
            {
                Debug.LogError("Sound Library is empty: Add some sounds!");
                return true;
            }

            if (!_sounds.ContainsKey(sound))
            {
                Debug.LogError("No Sound in library: " + sound);
                return true;
            }

            return false;
        }
    }
}
