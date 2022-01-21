using System.IO;
using UnityEditor;
using UnityEngine;

namespace Audio_System
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerInspector : Editor
    {
        const string soundLibrary = "Assets/no-money-tools/Audio_System/Sound_Library";
        const string editorStr = "Audio Paths";

        private string soundPaths;
        private AudioManager manager;

        private void OnEnable()
        {
            manager = (AudioManager)target;
            soundPaths = EditorPrefs.GetString(editorStr);
        }

        public override void OnInspectorGUI()
        {
            if (manager == null) return;

            EditorGUILayout.LabelField("Sound Paths");
            EditorGUI.BeginChangeCheck();
            soundPaths = EditorGUILayout.TextArea(soundPaths);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(editorStr, soundPaths);
                Undo.RecordObject(manager, "Changed Sound Paths");
                EditorUtility.SetDirty(this);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Sounds"))
            {
                Debug.Log("Loading Sounds");
                string[] paths = soundPaths.Split('\n');

                foreach(string path in paths)
                {
                    string file = path.Substring(0, 2) == "./" ?
                        soundLibrary + path.Substring(1) :
                        path;

                    if(path == "Library" || path == "library")
                    {
                        LoadDirectory(soundLibrary);
                    }
                    else if(file[file.Length - 1] == '/')
                    {
                        // Directory
                        LoadDirectory(file);
                    }
                    else
                    {
                        // Individual Asset
                        Sound sound = AssetDatabase.LoadAssetAtPath<Sound>(file);
                        if (!manager.tempSounds.Contains(sound))
                        {
                            manager.tempSounds.Add(sound);
                        }
                    }

                }
                EditorUtility.SetDirty(manager);
            }
            if(GUILayout.Button("Clear Sounds"))
            {
                Debug.Log("Clearing Sounds");
                manager.tempSounds.Clear();
            }
            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// Loads all sounds in the given directory and sub directories.
        /// </summary>
        /// <param name="path">Path to the directory to load.</param>
        private void LoadDirectory(string path)
        {
            foreach(string file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file) == ".meta")
                {
                    continue;
                }
                else
                {
                    Sound sound = AssetDatabase.LoadAssetAtPath<Sound>(file);
                    if (!manager.tempSounds.Contains(sound))
                    {
                        manager.tempSounds.Add(sound);
                    }
                }
            }
            foreach(string directory in Directory.GetDirectories(path))
            {
                LoadDirectory(directory);
            }
        }
    }
}