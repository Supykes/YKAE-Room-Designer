using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Windows")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadDesignMenu;

    [Header("UI Save Files Parent")]
    [SerializeField] Transform saveFilesDisplayParent;

    [Header("UI Save Files")]
    [SerializeField] GameObject saveFileDisplay;
    DirectoryInfo directoryInfo;
    bool getSaveFilesOnce = false;

    void Awake()
    {
        directoryInfo = new(Application.persistentDataPath);

        PlayerPrefs.DeleteAll();
    }

    public void StartNewDesign()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void LoadDesign()
    {
        mainMenu.SetActive(false);
        loadDesignMenu.SetActive(true);

        GetSaveFiles();
    }

    public void GoBackToMainMenu()
    {
        mainMenu.SetActive(true);
        loadDesignMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void GetSaveFiles()
    {
        if (!getSaveFilesOnce)
        {
            getSaveFilesOnce = true;

            var fileInfos = directoryInfo.GetFiles().OrderBy(file => file.CreationTime);
            float saveFileDisplayPositionX = 50f;
            float saveFileDisplayPositionY = 90f;
            int saveFilesCount = 0;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Extension.Contains("dat") && saveFilesCount < 15)
                {
                    GameObject saveFileUI = Instantiate(saveFileDisplay, saveFileDisplay.transform.position + new Vector3(saveFileDisplayPositionX,
                    saveFileDisplayPositionY, 0f), saveFileDisplay.transform.rotation, saveFilesDisplayParent);
                    saveFileUI.SetActive(true);

                    saveFileUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fileInfo.Name;

                    saveFilesCount++;

                    saveFileDisplayPositionX += 300f;

                    if (saveFilesCount % 5 == 0)
                    {
                        saveFileDisplayPositionX = 50f;
                        saveFileDisplayPositionY -= 120f;
                    }
                }
            }
        }
    }

    public void LoadSaveFile()
    {
        GameObject selectedSaveFile = EventSystem.current.currentSelectedGameObject;

        PlayerPrefs.SetString("saveFileDestination", Application.persistentDataPath + "/" +
        selectedSaveFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

        SceneManager.LoadScene("Main Scene");
    }
}