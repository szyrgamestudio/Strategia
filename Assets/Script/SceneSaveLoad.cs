using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public class ObjectData
{
    public string objectName;
    public SerializableVector3 position;
    // Dodaj inne właściwości obiektu, jeśli są potrzebne (np. rotacja, skala itp.)
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public List<ObjectData> objectDataList = new List<ObjectData>();
}

public static class SceneSaveLoad
{
    private const string FileName = "save1b";

    public static void SaveScene()
    {
        SceneData sceneData = new SceneData();
        sceneData.sceneName = SceneManager.GetActiveScene().name;

        GameObject[] sceneObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            ObjectData objectData = new ObjectData();
            objectData.objectName = obj.name;
            objectData.position = new SerializableVector3(obj.transform.position);

            // Dodaj inne dane obiektu, jeśli są potrzebne

            sceneData.objectDataList.Add(objectData);
        }

        string filePath = Path.Combine(Application.dataPath, "Data", FileName);

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            formatter.Serialize(fileStream, sceneData);
        }

        Debug.Log("Scena została zapisana.");
    }

    public static void LoadScene()
    {
        string filePath = Path.Combine(Application.dataPath, "Data", FileName);

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                SceneData sceneData = (SceneData)formatter.Deserialize(fileStream);

                // Załaduj scenę
                SceneManager.LoadScene(sceneData.sceneName);

                // Przywróć obiekty na scenie
                foreach (ObjectData objectData in sceneData.objectDataList)
                {
                    GameObject obj = GameObject.Find(objectData.objectName);
                    if (obj != null)
                    {
                        obj.transform.position = objectData.position.ToVector3();

                        // Przywróć inne dane obiektu, jeśli są potrzebne
                    }
                }

                Debug.Log("Scena została wczytana.");
            }
        }
        else
        {
            Debug.LogError("Plik zapisu sceny nie istnieje.");
        }
    }
}
