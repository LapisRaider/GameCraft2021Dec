using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySaveFileFormatter
{
    public static void SavePlayer(SaveFile _saveFile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, _saveFile);
        stream.Close();
    }

    public static SaveFile LoadPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveFile data = formatter.Deserialize(stream) as SaveFile;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}