using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    public static string SavePath = Application.persistentDataPath + "/savefile.json";

    public static void SaveData(string data)
    {
        System.IO.File.WriteAllText(SavePath, data);
    }

    public static JSONObject LoadData()
    {
        if (System.IO.File.Exists(SavePath))
        {
            string dataString = System.IO.File.ReadAllText(SavePath);
            JSONObject data = JSONObject.Create(dataString);
            return data;
        }
        return null;
    }

    public static bool SaveFileExists()
    {
        return System.IO.File.Exists(SavePath);
    }

    public static void DeleteSaveFile()
    {
        if (System.IO.File.Exists(SavePath))
        {
            System.IO.File.Delete(SavePath);
        }
    }
}
