using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISaveable
{
    string Serialize();
}

public static class SaveManager
{
    public static bool HasSave(string fileName, out string savePath)
    {
        savePath = GetSaveDirectory(fileName);

        return File.Exists(savePath);
    }

    public static bool DeleteSave(string fileName)
    {
        if (HasSave(fileName, out string savePath))
        {
            File.Delete(savePath);

            return true;
        }

        return false;
    }

    public static string GetSaveDirectory()
    {
        string path = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
        string directory = Path.Combine(path, "Save");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
    }

    public static string GetSaveDirectory(string fileName)
    {
        return Path.Combine(GetSaveDirectory(), fileName);
    }

    public static bool Load<T>(string fileName, out T data) where T : ISaveable
    {
        data = default;

        string path = GetSaveDirectory(fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            try
            {
                object o = JsonUtility.FromJson<T>(json);
                data = (T)o;
                return true;
            }
            catch
            {
                Debug.LogWarning("invalid json");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static void Save<T>(string fileName, T data) where T : ISaveable
    {
        string path = GetSaveDirectory(fileName);
        File.WriteAllText(path, (data as ISaveable).Serialize());
    }
}
