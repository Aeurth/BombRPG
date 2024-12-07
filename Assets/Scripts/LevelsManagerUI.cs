using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManagerUI : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] GameObject content;
    [SerializeField] GameObject levelPrefab;

    LevelConfig[] LevelConfigs;
    private void Awake()
    {
        LevelUI.OnLevelSelected += StartLevel;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadLevelsConfigs();
        LoadLevels();
    }


    private void LoadLevelsConfigs()
    {
        LevelConfigs = Resources.LoadAll<LevelConfig>("LevelConfigs");
    }
    private void AddLevel(LevelConfig levelConfig)
    {
        GameObject level = Instantiate(levelPrefab, content.transform);
        level.GetComponent<LevelUI>().SetLevelName(levelConfig.levelName);

    }
    private void LoadLevels()
    {
        foreach (LevelConfig level in LevelConfigs) {
            AddLevel(level);
        }
    }
    private void StartLevel()
    {
        LevelUI.OnLevelSelected -= StartLevel;
        StartCoroutine(LoadYourAsyncScene(2));
    }
    IEnumerator LoadYourAsyncScene(int index)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
