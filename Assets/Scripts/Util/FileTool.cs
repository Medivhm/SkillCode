using System.IO;
using UnityEngine;

public class FileTool : Singleton<FileTool>
{

    public static bool HasFile(string p)
    {
        string path = p;
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public static void CopyFile(string from, string to)
    {
        File.Copy(from, to, true);
    }

    public static void DeleteFile(string p)
    {
        string path = p;

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}