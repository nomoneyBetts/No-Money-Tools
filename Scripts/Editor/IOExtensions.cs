using System.IO;
using UnityEditor;

namespace NoMoney.NoMoneyEditor
{
    public static class IOExtensions
    {
        /// <summary>
        /// Checks if a directory is at the path, otherwise makes it.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if path already existed, false if had to create.</returns>
        public static bool ConfirmDirectoryExists(this string path)
        {
            bool exists = Directory.Exists(path);
            if (!exists)
            {
                int i;
                for (i = path.Length - 1; i > -1; i--)
                {
                    if (path[i] == '\\')
                    {
                        break;
                    }
                }
                string parent = path.Remove(i);
                DirectoryInfo info = new DirectoryInfo(parent);
                AssetDatabase.CreateFolder(info.Parent.FullName.TrimToAssets(), info.Name);
            }
            return exists;
        }

        public static string TrimToAssets(this string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            while (info.Name != "Assets")
            {
                info = new DirectoryInfo(info.Parent.FullName);
            }

            string trimmed = $"Assets\\{path.Replace(info.FullName, "")}";
            return trimmed;
        }
    }
}
