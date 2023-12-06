using System.IO;
using UnityEngine;

public class SaveManager
{

    private string filePath;
    public SaveData saveData;
    
    public SaveManager(SaveData _saveData)
    {
        saveData = _saveData;
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
        // Debug.Log(filePath);
    }
    
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }
    
    // TODO: ファイルが破損していた場合とかのエラーハンドリングをした方がよい
    public void Load()
    { 
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            // save = JsonUtility.FromJson<SaveData>(data);
            JsonUtility.FromJsonOverwrite(data, saveData);
        }
    }

    public bool ExistsSaveFile(){
        return File.Exists(filePath);
    }

}