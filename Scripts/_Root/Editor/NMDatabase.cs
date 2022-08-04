// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace NoMoney
{
    /// <summary>
    /// Stores scriptable objects as assets to be used by NoMoney Systems.
    /// </summary>
    internal static class NMDatabase
    {
        public static readonly string Name = "NM-Database";

        public static string DBPath
        {
            get
            {
                string path = $"Assets/{Name}";
                if (!AssetDatabase.IsValidFolder(path))
                {
                    AssetDatabase.CreateFolder("Assets", Name);
                }
                return path;
            }
        }

        private static readonly Exception s_assetExists =
            new Exception("Asset already exists at that path.");

        /// <summary>
        /// Searches the NM-Database for the corresponding asset and returns it.
        /// </summary>
        /// <typeparam name="T">The type of asset to return.</typeparam>
        /// <param name="path">Path to the asset from its library (e.g. Audio/Test).</param>
        /// <returns>The found asset or default value.</returns>
        public static T LoadAsset<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>($"{DBPath}/{path}");
        }

        /// <summary>
        /// Rename an asset.
        /// </summary>
        /// <param name="obj">Object to rename</param>
        /// <param name="newName">New name for asset.</param>
        /// <exception cref="Exception">Fail to Rename.</exception>
        public static void RenameAsset(Object obj, string newName)
        {
            if (!Path.HasExtension(newName)) newName += ".asset";
            string path = AssetDatabase.GetAssetPath(obj);
            string[] parts = path.Split('/', '\\');
            if (parts[^1].ToLower() == newName.ToLower())
            {
                throw new Exception("Fail to Rename: Renaming to the same name");
            }

            parts[^1] = newName;
            string newPath = string.Join<string>('/', parts);
            if (File.Exists(newPath)) throw s_assetExists;

            string error = AssetDatabase.RenameAsset(path, newName);
            if (!string.IsNullOrEmpty(error)) throw new Exception(error);
        }

        /// <summary>
        /// Moves an asset or folder from one path to another.
        /// Creates any necessary folders.
        /// </summary>
        /// <param name="oldPath">The path to the asset.</param>
        /// <param name="newPath">The new path to the asset.</param>
        /// <exception cref="Exception">Fail to Move.</exception>
        public static void MoveAsset(string oldPath, string newPath)
        {
            if (!Path.HasExtension(oldPath)) oldPath += ".asset";
            if (!Path.HasExtension(newPath)) newPath += ".asset";
            if (oldPath == newPath) return;
            string fullOld = $"{DBPath}/{oldPath}";
            string fullNew = $"{DBPath}/{newPath}";

            if (PathExists(newPath)) throw s_assetExists;

            CreateFoldersInHeirarchy($"{newPath}");
            string error = AssetDatabase.MoveAsset(fullOld, fullNew);
            if (!string.IsNullOrEmpty(error)) throw new Exception(error);
            DeleteEmptyDirectories(fullOld);
        }

        /// <summary>
        /// Creates an asset for the object at the path.
        /// Creates any necessary folders.
        /// </summary>
        /// <param name="asset">The asset to create.</param>
        /// <param name="path">The path to the asset.</param>
        /// <exception cref="Exception">Fail to Create.</exception>
        public static void CreateAsset(Object asset, string path)
        {
            if (PathExists(path)) throw s_assetExists;
            CreateFoldersInHeirarchy(path);
            string fullPath = $"{DBPath}/{path}";
            if (!Path.HasExtension(fullPath)) fullPath += ".asset";
            AssetDatabase.CreateAsset(asset, fullPath);
        }

        /// <summary>
        /// Deletes an asset.
        /// </summary>
        /// <param name="path">The path to the asset.</param>
        /// <exception cref="Exception">Fail to Delete.</exception>
        public static void DeleteAsset(string path)
        {
            string fullPath = $"{DBPath}/{path}";
            if (!Path.HasExtension(fullPath)) fullPath += ".asset";

            if (!AssetDatabase.DeleteAsset(fullPath))
            {
                throw new Exception("Failed to delete: Asset may not exist");
            }
            DeleteEmptyDirectories(fullPath);
        }

        /// <param name="path">The path in question with root at the library in the database.</param>
        /// <returns>True if the given path exists; false otherwise.</returns>
        public static bool PathExists(string path)
        {
            return File.Exists($"{Directory.GetCurrentDirectory()}/{DBPath}/{path}");
        }

        private static string BFS(string target, Queue<string> directories)
        {
            string curDir;
            if (!directories.TryDequeue(out curDir)) return null;

            foreach (string dir in AssetDatabase.GetSubFolders(curDir))
            {
                string dirName = dir.Split('/', '\\')[^1];
                if (dirName == target) return dir;
                directories.Enqueue(dir);
            }

            return BFS(target, directories);
        }

        private static void CreateFoldersInHeirarchy(string path)
        {
            string[] heirarchy = path.Split('/', '\\');
            string parentdir = DBPath;

            for (int i = 0; i < heirarchy.Length - 1; i++)
            {
                string childdir = $"{parentdir}/{heirarchy[i]}";
                if (!AssetDatabase.IsValidFolder(childdir))
                {
                    string error = AssetDatabase.CreateFolder(parentdir, heirarchy[i]);
                    if (string.IsNullOrEmpty(error)) throw new Exception("Fail to Create Folder.");
                }
                parentdir = childdir;
            }
        }

        private static void DeleteEmptyDirectories(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                path = GetParent(path);
            }
            DirectoryInfo info = new DirectoryInfo(path);

            while (info.GetDirectories().Length == 0 && info.GetFiles().Length == 0)
            {
                AssetDatabase.DeleteAsset(path);
                path = GetParent(path);
                info = info.Parent;
            }

            static string GetParent(string path)
            {
                int stop = 0;
                for(int i = path.Length - 1; i >= 0; i--)
                {
                    if (path[i] == '/' || path[i] == '\\')
                    {
                        stop = i;
                        break;
                    }
                }
                return path.Substring(0, stop);
            }
        }
    }
}
