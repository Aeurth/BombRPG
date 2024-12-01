using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static event Action OnUIManagerInitialised;
    public static UIManager Instance { get; private set; }

    [Header("UI references")]
    [SerializeField] GameObject levelsList;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI objectsCount;
    [SerializeField] TextMeshProUGUI bombsCount;
    [SerializeField] TextMeshProUGUI maxBombsCount;
    [SerializeField] GameObject levelComplePopUp;

    [Header("Prefabs")]
    [SerializeField] GameObject levelListItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: if you want the instance to persist across scenes
    }
    private void OnEnable()
    {
        LevelManager.OnLevelDataChanged += UpdateLevelData;
        BombManager.OnBombsCountChanged += UpdateBombsCount;
        LevelManager.levelComplete += OnLevelCompleted;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelDataChanged -= UpdateLevelData;
        LevelManager.levelComplete -= OnLevelCompleted;
    }
    private void Start()
    {
        OnUIManagerInitialised?.Invoke();
    }
    public void UpdateObsticlesCount(int count)
    {
        objectsCount.text = count.ToString();
    }
    public void UpdateLevelName(string name) 
    {
        levelName.text = name;
    }
    public void UpdateMaxBombsCount(int count)
    {
        maxBombsCount.text = count.ToString();
    }
    public void UpdateBombsCount(int count)
    {
        bombsCount.text = count.ToString();
    }
    public void AddToLevels(LevelConfig[] LevelConfigs)
    {
        GameObject levelItem;
        Transform levelName;

        foreach (LevelConfig level in LevelConfigs)
        {
            levelItem = Instantiate(levelListItem, levelsList.transform);

            levelName = levelItem.transform.GetChild(0);
            levelName.GetComponent<Text>().text = level.name;

        }
    }
    private void OnLevelCompleted()
    {
        levelsList.SetActive(false);
        levelComplePopUp.SetActive(true);
    }

    public void UpdateData(GameData gameData)
    {
        UpdateLevelName(gameData.LevelName);
        UpdateBombsCount(gameData.PlacedBombCount);
        UpdateMaxBombsCount(gameData.MaxBombCount);
        UpdateObsticlesCount(gameData.ObsticlesCount);

    }
    public void UpdateLevelData(LevelData data)
    {
        levelName.text = data.levelName;
        objectsCount.text = data.obsticlesCount.ToString();
    }
}
