using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/* Save and Load tutorial Reference https://www.youtube.com/watch?v=XOjd_qU2Ido&t=651s
*/
public static class SaveSystem
{
    public static void CreateNewFile(GameManager player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerSaveFile";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void SavePlayer(GameManager player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerSaveFile";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerSaveFile";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File not found in " + path);
            return null;
        }
    }
}
