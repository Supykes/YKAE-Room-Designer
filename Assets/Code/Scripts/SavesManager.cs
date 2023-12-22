using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    [Header("Objects Parent")]
    [SerializeField] Transform objects;

    [Header("Objects To Spawn")]
    [SerializeField] GameObject bookcase;
    [SerializeField] GameObject table;
    [SerializeField] GameObject restChair;

    [Header("UI Text")]
    [SerializeField] GameObject saveConfirmTextBox;
    DirectoryInfo directoryInfo;
    int savesCount;
    string saveFileDestination;

    void Awake()
    {
        directoryInfo = new(Application.persistentDataPath);
        saveFileDestination = PlayerPrefs.GetString("saveFileDestination");
    }

    void Start()
    {
        GetSavesCount();

        LoadRoomDesign();
    }

    void Update()
    {
        SaveRoomDesign();
    }

    void SaveRoomDesign()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string newSaveFileDestination = Application.persistentDataPath + "/RoomDesign" + savesCount + ".dat";
            FileStream saveFile = File.Create(newSaveFileDestination);

            BinaryFormatter binaryFormatter = new();

            foreach (ObjectData objectData in ObjectsManager.objectsData)
            {
                binaryFormatter.Serialize(saveFile, objectData);
            }
            saveFile.Close();

            savesCount++;

            ShowConfirmationText();
            Invoke(nameof(HideConfirmationText), 2f);
        }
    }

    void LoadRoomDesign()
    {
        GetObjectsDataFromSaveFile();

        FormObjectsList();
    }

    void GetObjectsDataFromSaveFile()
    {
        if (saveFileDestination != "")
        {
            FileStream saveFile = File.OpenRead(saveFileDestination);

            BinaryFormatter binaryFormatter = new();

            while (saveFile.Position != saveFile.Length)
            {
                ObjectData objectData = (ObjectData)binaryFormatter.Deserialize(saveFile);
                ObjectsManager.objectsData.Add(objectData);
            }

            saveFile.Close();
        }
    }

    void FormObjectsList()
    {
        foreach (ObjectData objectData in ObjectsManager.objectsData)
        {
            int id = objectData.id;
            string type = objectData.type;
            GameObject gameObjectType = null;
            Vector3 location = new(objectData.locationX, objectData.locationY, objectData.locationZ);
            Color colour = new(objectData.colourR, objectData.colourG, objectData.colourB, objectData.colourA);

            switch (type)
            {
                case "Bookcase(Clone)":
                    gameObjectType = bookcase;
                    break;
                case "Table(Clone)":
                    gameObjectType = table;
                    break;
                case "Rest Chair(Clone)":
                    gameObjectType = restChair;
                    break;
            }

            SpawnSavedObject(id, gameObjectType, location, colour);
        }
    }

    void SpawnSavedObject(int id, GameObject gameObjectType, Vector3 location, Color colour)
    {
        if (gameObjectType)
        {
            GameObject spawnedSavedObject = Instantiate(gameObjectType, location, Quaternion.Euler(new Vector3(-90f, 0f, 0f)), objects);
            spawnedSavedObject.tag = "Placed Object";
            spawnedSavedObject.GetComponent<Object>().SetObjectValues(id, gameObjectType, location, colour);
            spawnedSavedObject.GetComponent<Renderer>().material.SetColor("_Color", colour);
            spawnedSavedObject.GetComponent<MeshCollider>().isTrigger = true;
            spawnedSavedObject.AddComponent<RoomCollision>();

            ObjectsManager.objects.Add(spawnedSavedObject);
        }
        else
        {
            ObjectsManager.objects.Add(null);
        }
    }

    void GetSavesCount()
    {
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension.Contains("dat"))
            {
                savesCount++;
            }
        }
    }

    void ShowConfirmationText()
    {
        saveConfirmTextBox.SetActive(true);
    }

    void HideConfirmationText()
    {
        saveConfirmTextBox.SetActive(false);
    }
}