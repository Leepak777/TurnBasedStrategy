using UnityEngine;
using System.IO;

public class ScriptableObjectManager
{
    private string dataPath; // The path where the ScriptableObject data will be saved

    public ScriptableObjectManager(string dataPath)
    {
        this.dataPath = dataPath;
    }

    public void CreateAndSaveScriptableObject(ScriptableObject scriptableObject, string fileName)
    {
        string filePath = Path.Combine("Assets/Scripts/Data/", fileName);

        

        // Serialize the ScriptableObject data to JSON
        string json = JsonUtility.ToJson(scriptableObject);

        // Save the JSON data to a file
        File.WriteAllText(filePath, json);
    }

    public T LoadScriptableObject<T>(string fileName) where T : ScriptableObject
    {
        string filePath = Path.Combine("Assets/Scripts/Data/", fileName);

        // Check if the asset file already exists
        T existingAsset = Resources.Load<T>(fileName);
        if (existingAsset != null)
        {
            return existingAsset;
        }

        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(filePath);

            if (!string.IsNullOrEmpty(json))
            {
                // Create a new instance of the ScriptableObject
                T scriptableObject = ScriptableObject.CreateInstance<T>();

                // Apply the serialized JSON data to the ScriptableObject
                JsonUtility.FromJsonOverwrite(json, scriptableObject);

                return scriptableObject;
            }
        }

        Debug.LogError("Failed to load ScriptableObject: " + fileName);
        return null;
    }



    public string[] FindFilesByName(string fileName)
    {
        DirectoryInfo directory = new DirectoryInfo(dataPath);
        FileInfo[] files = directory.GetFiles(fileName + ".json", SearchOption.AllDirectories);

        string[] fileNames = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            fileNames[i] = Path.GetFileNameWithoutExtension(files[i].Name);
        }

        return fileNames;
    }

    public void DeleteScriptableObject(string fileName)
    {
        string filePath = Path.Combine("Assets/Scripts/Data/", fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public void DeleteAllAssetsWithSubstring(string substring)
    {
        DirectoryInfo directory = new DirectoryInfo(dataPath);
        FileInfo[] files = directory.GetFiles("*" + substring + "*.json", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i].FullName);
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(dataPath, fileName + ".json");
    }
}
