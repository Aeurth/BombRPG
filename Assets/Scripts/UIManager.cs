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
        BombManager.OnBombsDataChanged += UpdateBombsData;
        LevelManager.levelComplete += OnLevelCompleted;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelDataChanged -= UpdateLevelData;
        LevelManager.levelComplete -= OnLevelCompleted;
        BombManager.OnBombsDataChanged -= UpdateBombsData;
    }
    private void Start()
    {
        OnUIManagerInitialised?.Invoke();
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

    public void UpdateLevelData(LevelData data)
    {
        levelName.text = data.levelName;
        objectsCount.text = data.obsticlesCount.ToString();
    }
    public void UpdateBombsData(BombsData data)
    {
        maxBombsCount.text = data.maxBombsCount.ToString();
        bombsCount.text = data.bombsCount.ToString();

    }
}
