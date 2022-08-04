// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Object = UnityEngine.Object;
using UnityEngine.Audio;

namespace NoMoney.Audio
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerInspector : Editor
    {
        private const string SoundLibrary = "Sound-Library";

        private const string SoundName = "sound-name";
        private const string Objs = "objects";
        private const string Bools = "bools";
        private const string Sliders = "sliders";
        private const string Sounds = "_sounds";
        private const string SelectedSound = "_selectedSound";
        private const string LoadedSounds = "_loadedSounds";

        private VisualElement _inspector;
        private readonly List<IDisposable> _tokens = new List<IDisposable>();
        private string _nameBuffer;

        public override VisualElement CreateInspectorGUI()
        {
            _inspector = new VisualElement();
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("AudioManager");
            visualTree.CloneTree(_inspector);
            _inspector.styleSheets.Add(Resources.Load<StyleSheet>("AudioManager"));

            DropdownListField loadedSounds = _inspector.Q<DropdownListField>();
            loadedSounds.SetValueWithoutNotify(serializedObject.FindProperty(LoadedSounds).GetValue<List<string>>());
            loadedSounds.RegisterValueChangedCallback(LoadSounds);

            _inspector.Q<Button>("add-sound").clicked += AddSound_clicked;
            _inspector.Q<Button>("delete-sound").clicked += DeleteSound_clicked;
            _inspector.Q<Button>("submit").clicked += Submit_clicked;
            _inspector.Q<Button>("play-sound").clicked += () =>
            {
                Sound sound = serializedObject.FindProperty(SelectedSound).GetValue<Sound>();
                if (sound == null) return;
                AudioManager manager = (AudioManager)target;
                AudioSource source = GetSource(manager.gameObject);
                sound.Apply(source);
                source.Play();
            };
            _inspector.Q<Button>("stop-sound").clicked += () =>
            {
                AudioManager manager = (AudioManager)target;
                AudioSource source = GetSource(manager.gameObject);
                source.Stop();
            };

            PopulateInspector();
            return _inspector;

            static AudioSource GetSource(GameObject go)
            {
                AudioSource source;
                if ((source = go.GetComponent<AudioSource>()) == null)
                {
                    source = go.gameObject.AddComponent<AudioSource>();
                }
                return source;
            }
        }

        #region Button Clicks
        private void Submit_clicked()
        {
            Sound sound = serializedObject.FindProperty(SelectedSound).GetValue<Sound>();
            if (sound == null) return;

            // Error checking
            if (string.IsNullOrEmpty(_nameBuffer))
            {
                Debug.LogError("Cannot submit empty names");
                return;
            }

            // Rename the asset
            string[] parts = _nameBuffer.Split('/', '\\');
            NMDatabase.RenameAsset(sound, parts[^1]);

            // Move the asset
            string[] oldParts = sound.Name.Split('/', '\\');
            oldParts[^1] = parts[^1];
            string oldName = string.Join<string>('/', oldParts);
            NMDatabase.MoveAsset($"{SoundLibrary}/{oldName}", $"{SoundLibrary}/{_nameBuffer}");

            // Update the asset
            sound.Name = _nameBuffer;
            EditorUtility.SetDirty(sound);
            UpdateLoadSounds();
            PopulateInspector();
        }

        private void DeleteSound_clicked()
        {
            Sound sound = serializedObject.FindProperty(SelectedSound).GetValue<Sound>();
            if (sound != null)
            {
                StringSoundDict sounds = serializedObject.FindProperty(Sounds).GetValue<StringSoundDict>();
                sounds.Remove(sound.Name);
                NMDatabase.DeleteAsset($"{SoundLibrary}/{sound.Name}");
                sound.DestroySound();
            }
            serializedObject.FindProperty(SelectedSound).SetValue(null);
            UpdateLoadSounds();
            PopulateInspector();
        }

        private void AddSound_clicked()
        {
            if (NMDatabase.PathExists($"{SoundLibrary}/NAME ME.asset"))
            {
                Debug.LogError("Faild to add new sound: You forgot to name an asset!");
                return;
            }

            Sound sound = CreateInstance<Sound>();
            NMDatabase.CreateAsset(sound, $"{SoundLibrary}/{sound.Name}");
            serializedObject.FindProperty(SelectedSound).SetValue(sound);
            UpdateLoadSounds();
            PopulateInspector();
        }
        #endregion

        /// <summary>
        /// Populates the Inspector with the Selected Sound
        /// </summary>
        private void PopulateInspector()
        {
            Sound sound = serializedObject.FindProperty(SelectedSound).GetValue<Sound>();
            _tokens.ForEach(t => t.Dispose());
            if (sound == null)
            {
                UpdateSelector();
                return;
            }

            UpdateSelector(sound.Name);

            EventCallback<ChangeEvent<string>> stringChange;
            EventCallback<ChangeEvent<bool>> boolChange;
            EventCallback<ChangeEvent<int>> intChange;
            EventCallback<ChangeEvent<float>> floatChange;
            EventCallback<ChangeEvent<Object>> objChange;

            // Sound Name
            TextField soundName = _inspector.Q<TextField>(SoundName);
            soundName.SetValueWithoutNotify(sound.Name);
            soundName.RegisterValueChangedCallback(stringChange = evt =>
            {
                _nameBuffer = evt.newValue;
            });
            _tokens.Add(new ChangeEventRegistrationToken<string>() { Element = soundName, Event = stringChange });

            #region Objs
            VisualElement objs = _inspector.Q<VisualElement>(Objs);
            int index = 0;

            // Audio Clip
            ObjectField objField = (ObjectField)objs.ElementAt(index++);
            objField.SetValueWithoutNotify(sound.Clip);
            objField.RegisterValueChangedCallback(objChange = evt =>
            {
                sound.Clip = (AudioClip)evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<Object>() { Element = objField, Event = objChange });

            // Audio Clip
            objField = (ObjectField)objs.ElementAt(index++);
            objField.SetValueWithoutNotify(sound.Output);
            objField.RegisterValueChangedCallback(objChange = evt =>
            {
                sound.Output = (AudioMixerGroup)evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<Object>() { Element = objField, Event = objChange });
            #endregion

            #region Bools
            VisualElement bools = _inspector.Q<VisualElement>(Bools);
            index = 0;

            // Mute
            Toggle toggle = (Toggle)bools.ElementAt(index++);
            toggle.SetValueWithoutNotify(sound.Mute);
            toggle.RegisterValueChangedCallback(boolChange = evt =>
            {
                sound.Mute = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<bool>() { Element = toggle, Event = boolChange });

            // Bypass Efx
            toggle = (Toggle)bools.ElementAt(index++);
            toggle.SetValueWithoutNotify(sound.BypassEffects);
            toggle.RegisterValueChangedCallback(boolChange = evt =>
            {
                sound.BypassEffects = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<bool>() { Element = toggle, Event = boolChange });

            // Bypass Listener Efx
            toggle = (Toggle)bools.ElementAt(index++);
            toggle.SetValueWithoutNotify(sound.BypassListenerEffects);
            toggle.RegisterValueChangedCallback(boolChange = evt =>
            {
                sound.BypassListenerEffects = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<bool>() { Element = toggle, Event = boolChange });

            // Bypass Reverb Zones
            toggle = (Toggle)bools.ElementAt(index++);
            toggle.SetValueWithoutNotify(sound.BypassReverbZone);
            toggle.RegisterValueChangedCallback(boolChange = evt =>
            {
                sound.BypassReverbZone = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<bool>() { Element = toggle, Event = boolChange });

            // Play On Awake
            toggle = (Toggle)bools.ElementAt(index++);
            toggle.SetValueWithoutNotify(sound.PlayOnAwake);
            toggle.RegisterValueChangedCallback(boolChange = evt =>
            {
                sound.PlayOnAwake = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<bool>() { Element = toggle, Event = boolChange });

            // Loop
            toggle = (Toggle)bools.ElementAt(index++);
            toggle.SetValueWithoutNotify(sound.Loop);
            toggle.RegisterValueChangedCallback(boolChange = evt =>
            {
                sound.Loop = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<bool>() { Element = toggle, Event = boolChange });
            #endregion

            #region Sliders
            VisualElement sliders = _inspector.Q<VisualElement>(Sliders);
            index = 0;

            // Priority
            NMSliderInt sliderInt = (NMSliderInt)sliders.ElementAt(index++);
            sliderInt.SetValueWithoutNotify(sound.Priority);
            sliderInt.RegisterValueChangedCallback(intChange = evt =>
            {
                sound.Priority = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<int>() { Element = sliderInt, Event = intChange });

            // Volume
            NMSlider slider = (NMSlider)sliders.ElementAt(index++);
            slider.SetValueWithoutNotify(sound.Volume);
            slider.RegisterValueChangedCallback(floatChange = evt =>
            {
                sound.Volume = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<float>() { Element = slider, Event = floatChange });

            // Pitch
            slider = (NMSlider)sliders.ElementAt(index++);
            slider.SetValueWithoutNotify(sound.Pitch);
            slider.RegisterValueChangedCallback(floatChange = evt =>
            {
                sound.Pitch = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<float>() { Element = slider, Event = floatChange });

            // Stereo Pan
            slider = (NMSlider)sliders.ElementAt(index++);
            slider.SetValueWithoutNotify(sound.StereoPan);
            slider.RegisterValueChangedCallback(floatChange = evt =>
            {
                sound.StereoPan = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<float>() { Element = slider, Event = floatChange });

            // Spatial Blend
            slider = (NMSlider)sliders.ElementAt(index++);
            slider.SetValueWithoutNotify(sound.SpatialBlend);
            slider.RegisterValueChangedCallback(floatChange = evt =>
            {
                sound.SpatialBlend = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<float>() { Element = slider, Event = floatChange });

            // Reverb Zone Mix
            slider = (NMSlider)sliders.ElementAt(index++);
            slider.SetValueWithoutNotify(sound.ReverbZoneMix);
            slider.RegisterValueChangedCallback(floatChange = evt =>
            {
                sound.ReverbZoneMix = evt.newValue;
                EditorUtility.SetDirty(sound);
            });
            _tokens.Add(new ChangeEventRegistrationToken<float>() { Element = slider, Event = floatChange });
            #endregion
        }

        /// <summary>
        /// Update the Sound Selector
        /// </summary>
        /// <param name="text">Text to display</param>
        private void UpdateSelector(string text = null)
        {
            const string unset = "UNSET";
            ToolbarMenu selector = _inspector.Q<ToolbarMenu>("sound-selector");
            selector.ClearMenu();
            selector.text = text ?? unset;

            // Unset action
            selector.menu.AppendAction(unset, a =>
            {
                serializedObject.FindProperty(SelectedSound).SetValue(null);
                PopulateInspector();
            });

            // Scan Library
            string projdir = Directory.GetCurrentDirectory();
            string startdir = $"{projdir}/{NMDatabase.DBPath}/{SoundLibrary}";
            if (Directory.Exists(startdir)) LibraryScan(startdir);

            void LibraryScan(string curdir)
            {
                string soundParent = Path.GetFileName(curdir) == "Sound-Library" ?
                    null : curdir.Replace(startdir + '/', "");
                foreach (string path in Directory.GetFiles(curdir))
                {
                    if (Path.GetExtension(path) == ".meta") continue;
                    string file = Path.GetFileNameWithoutExtension(path);
                    string name = soundParent == null ? file : $"{soundParent}/{file}";

                    selector.menu.AppendAction(name, a =>
                    {
                        Sound sound = AssetDatabase.LoadAssetAtPath<Sound>(path.Replace(projdir + '/', ""));
                        serializedObject.FindProperty(SelectedSound).SetValue(sound);
                        PopulateInspector();
                    });
                }

                foreach (string dir in Directory.GetDirectories(curdir))
                {
                    LibraryScan($"{curdir}/{Path.GetFileName(dir)}");
                }
            }
        }

        /// <summary>
        /// Loads sounds into the AudioManager
        /// </summary>
        /// <param name="evt"></param>
        private void LoadSounds(ChangeEvent<List<string>> evt)
        {  
            StringSoundDict sounds = (StringSoundDict)serializedObject.FindProperty(Sounds).GetValue();
            if (sounds == null) sounds = new StringSoundDict();
            sounds.Clear();
            if (evt.newValue == null)
            {
                serializedObject.FindProperty(LoadedSounds).SetValue(null);
                return;
            }

            // Group sounds together who share a parent directory
            Dictionary<string, List<string>> soundGroups = new Dictionary<string, List<string>>();
            foreach (string sound in evt.newValue)
            {
                if (sound == null) continue;

                // collect parent directory
                int index = -1;
                for(int i = sound.Length - 1; i >= 0; i--)
                {
                    if (sound[i] == '/' || sound[i] == '\\')
                    {
                        index = i;
                        break;
                    }
                }
                string parent = index < 0 ? "" : sound[..index];
                string soundName = index < 0 ? sound : sound[(index + 1)..];

                if (soundGroups.ContainsKey(parent))
                {
                    soundGroups[parent].Add(soundName);
                }
                else
                {
                    List<string> group = new List<string>() { soundName };
                    soundGroups.Add(parent, group);
                }
            }

            // Load sounds from each group
            string projdir = Directory.GetCurrentDirectory();
            string libdir = $"{NMDatabase.DBPath}/{SoundLibrary}";
            string fullLibDir = $"{projdir}/{libdir}";
            List<string> loadedSounds = new List<string>();
            bool discardedSounds = false;

            foreach (KeyValuePair<string, List<string>> kvp in soundGroups)
            {
                // First scan for an ALL
                // If found, load all sounds from the key and discard the rest of the group
                if (kvp.Value.Find(s => s == "ALL") != null)
                {
                    LoadAll($"{fullLibDir}/{kvp.Key}");
                    string allPath = kvp.Key == "" ? "ALL" : $"{kvp.Key}/ALL";

                    // Remove all loaded sounds following this ALL
                    if (kvp.Key == "")
                    {
                        loadedSounds.Clear();
                        discardedSounds = true;
                        loadedSounds.Add(allPath);
                        break;
                    }
                    else if(loadedSounds.RemoveAll(s => s.Contains($"{kvp.Key}/")) > 0)
                    {
                        discardedSounds = true;
                    }

                    loadedSounds.Add(allPath);
                    if (kvp.Value.Count > 1) discardedSounds = true;
                }
                else
                {
                    foreach(string sound in kvp.Value)
                    {
                        string soundPath = kvp.Key == "" ?
                            $"{libdir}/{sound}" :
                            $"{libdir}/{kvp.Key}/{sound}";
                        loadedSounds.Add(soundPath);
                        soundPath += ".asset";
                        Sound s = AssetDatabase.LoadAssetAtPath<Sound>(soundPath);
                        sounds.TryAdd(s.Name, s);
                    }
                }
            }

            serializedObject.FindProperty(Sounds).SetValue(sounds);
            serializedObject.FindProperty(LoadedSounds).SetValue(loadedSounds);
            if (discardedSounds)
            {
                DropdownListField field = _inspector.Q<DropdownListField>();
                field.SetValueWithoutNotify(loadedSounds);
            }

            void LoadAll(string root)
            {
                // Load all the files
                foreach(string file in Directory.GetFiles(root))
                {
                    if (Path.GetExtension(file) != ".asset") continue;
                    string assetName = file[projdir.Length..];
                    if (assetName[0] == '/' || assetName[0] == '\\') assetName = assetName[1..];
                    Sound sound = AssetDatabase.LoadAssetAtPath<Sound>(assetName);
                    sounds.TryAdd(sound.Name, sound);
                }

                // Recurse through the directories
                foreach(string dir in Directory.GetDirectories(root))
                {
                    LoadAll(dir);
                }
            }
        }

        /// <summary>
        /// Updates the Load Sound list
        /// Called on sound added/removed/name changed.
        /// </summary>
        private void UpdateLoadSounds()
        {
            DropdownListField loadedSounds = _inspector.Q<DropdownListField>();
            loadedSounds.SetValueWithoutNotify(serializedObject.FindProperty(LoadedSounds).GetValue<List<string>>());
        }
    }
}
