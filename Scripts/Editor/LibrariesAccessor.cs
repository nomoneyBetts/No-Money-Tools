using System.IO;
using UnityEditor;

internal static class LibrariesAccessor
{
    public enum Libraries
    {
        Sequencer_Library,
        Sound_Library
    }

    /// <summary>
    /// Confirms the library exists, if not it is created.
    /// </summary>
    /// <param name="lib"></param>
    /// <returns>Path from Assets to the library.</returns>
    public static string SetLibrary(Libraries lib)
    {
        string curDir = Directory.GetCurrentDirectory();
        string path = curDir + '\\' + noMoneyLibrary + '\\' + lib.ToString();
        if (!Directory.Exists(path))
        {
            AssetDatabase.CreateFolder(noMoneyLibrary, lib.ToString());
        }
        return path.Replace(curDir + '\\', "");
    }

    public static string noMoneyRoot
    {
        get
        {
            string curDir = Directory.GetCurrentDirectory();
            string path = Directory.GetDirectories(curDir, "no-money-tools", SearchOption.AllDirectories)[0];
            return path.Replace(curDir + '\\', "");
        }
    }

    public static string noMoneyLibrary
    {
        get
        {
            string curDir = Directory.GetCurrentDirectory();
            string lib = curDir + "\\Assets\\NoMoney_Library";
            if (!Directory.Exists(lib))
            {
                AssetDatabase.CreateFolder("Assets", "NoMoney_Library");
            }
            return lib.Replace(curDir + '\\', "");
        }
    }

    public static string seqLib
    {
        get
        {
            return SetLibrary(Libraries.Sequencer_Library);
        }
    }

    public static string soundLib
    {
        get
        {
            return SetLibrary(Libraries.Sound_Library);
        }
    }

}
