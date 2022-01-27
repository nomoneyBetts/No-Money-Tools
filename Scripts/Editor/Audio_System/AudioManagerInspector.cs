using System.IO;
using UnityEditor;
using UnityEngine;

namespace Audio_System
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerInspector : Editor
    {
        private AudioManager manager;
        private SerializedProperty paths;

        private void OnEnable()
        {
            manager = (AudioManager)target;
            LibrariesAccessor.SetLibrary(LibrariesAccessor.Libraries.Sound_Library);
            paths = serializedObject.FindProperty("soundPaths");
        }

        public override void OnInspectorGUI()
        {
            if (manager == null) return;

            EditorGUILayout.LabelField("Sound Paths");
            EditorGUI.BeginChangeCheck();
            paths.stringValue = EditorGUILayout.TextArea(paths.stringValue);
            if (EditorGUI.EndChangeCheck())
            { 
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Sounds"))
            {
                Debug.Log("Loading Sounds");
                string soundLibrary = LibrariesAccessor.soundLib;
                string[] paths = this.paths.stringValue.Split('\n');

                foreach(string path in paths)
                {
                    string file = path.Substring(0, 2) == "./" ?
                        soundLibrary + path.Substring(1) :
                        path;

                    if(path == "Library" || path == "library")
                    {
                        LoadDirectory(soundLibrary);
                    }
                    else if(Directory.Exists(file))
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
                if (manager.tempSounds != null)
                {
                    manager.tempSounds.Clear();
                }
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