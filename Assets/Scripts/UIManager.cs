using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.RestService;
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
    [SerializeField] GameObject playerHealtContainer;

    [Header("Prefabs")]
    [SerializeField] GameObject levelListItem;
    [SerializeField] GameObject heartUI;

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
        PlayerController.PlayerDataChanged += UpdatePlayerData;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelDataChanged -= UpdateLevelData;
        LevelManager.levelComplete -= OnLevelCompleted;
        BombManager.OnBombsDataChanged -= UpdateBombsData;
        PlayerController.PlayerDataChanged -= UpdatePlayerData;

    }
    private void Start()
    {
        OnUIManagerInitialised?.Invoke();
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
    public void UpdatePlayerData(PlayerData data)
    {
        UpdatePlayerHealthUI(data.health);
    }
    private void DestroyHearts(int count = 1)
    {
        if (playerHealtContainer.transform.childCount > 0)
        {
            GameObject heart = playerHealtContainer.transform.GetChild(0).gameObject;
            Destroy(heart);
        }
    }
    private void AddHearts(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(heartUI, playerHealtContainer.transform);
        }
    }
    private void UpdatePlayerHealthUI(int updateCount)
    {
        if (updateCount > 0)
        {
            AddHearts(updateCount);
        }
        else if (updateCount < 0)
        {
            DestroyHearts(updateCount);
        }

    }
}
