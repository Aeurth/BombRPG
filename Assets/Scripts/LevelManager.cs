using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager: MonoBehaviour
{
    public static event Action<LevelData> OnLevelDataChanged;
    public static event Action OnConfigsSet;
    public static event Action levelComplete;

    LevelConfig[] LevelConfigs;
    LevelConfig currentLevel;
    Vector2Int[] spawnPoints;
    bool isConfigsSet = false;
    int DestructablesCount;

    [Header("Player stuff")]
    [SerializeField] GameObject player;

     


    private void OnEnable()
    {
        // Subscribe to events
        GridManager.OnGridManagerInitialized += HandleGridManagerInitialized;
        UIManager.OnUIManagerInitialised += HandleUIOnStart;
        BombManager.OnDestructibleDestroyed += HandleDestuctibleEvent;


    }
    private void Start()
    {
        Debug.Log("LevelManager initialised");
        LoadAllLevelsConfigs();
        SetUpLevelConfigs();
        SpawnPlayer(0);
        

    }
    private void LoadAllLevelsConfigs()
    {
        LevelConfigs = Resources.LoadAll<LevelConfig>("LevelConfigs");

    }
    private void SetUpLevelConfigs()
    {
        currentLevel = LevelConfigs[0];
        DestructablesCount = currentLevel.DestructiblesCount;
        spawnPoints = currentLevel.SpawnPoints;
        isConfigsSet= true;
        OnConfigsSet?.Invoke();
        Debug.Log("Level configs are setup");
    }
    private void HandleGridManagerInitialized()
    {
        // Perform actions that depend on the GridManager
        FillLevelWithItems();
        FillLevelWithPowerUps();
    }
    private void HandleUIOnStart()
    {

        if (isConfigsSet)
        {
            // Configurations are loaded, update the UI
            LevelData levelData = new LevelData(currentLevel.levelName, currentLevel.DestructiblesCount);
            OnLevelDataChanged?.Invoke(levelData);
            // Unsubscribe from the event after handling the UI
            OnConfigsSet -= HandleUIOnStart;
        }
        else // Subscribe to the event only if configs aren't loaded yet
        {
 
            OnConfigsSet -= HandleUIOnStart; // Ensure it's not subscribed multiple times
            OnConfigsSet += HandleUIOnStart;
        }
    }
    private void FillLevelWithItems()
    {
        GridManager.Instance.FillGridWithItems(DestructablesCount, spawnPoints);
    }    
    private void FillLevelWithPowerUps()
    {
        GridManager.Instance.FillGridWithPowerUp("ExplosionRadius", currentLevel.PWradiusCount);
        GridManager.Instance.FillGridWithPowerUp("BombCount", currentLevel.PWmaxBombsCount);
        GridManager.Instance.FillGridWithPowerUp("PlayerSpeed", currentLevel.PWspeedCount);
    }
    private void HandleDestuctibleEvent()
    {
        DestructablesCount--;
        LevelData data = new LevelData(currentLevel.levelName, DestructablesCount);
        OnLevelDataChanged?.Invoke(data);
       
        if (DestructablesCount == 0)
        {
            levelComplete?.Invoke();
        }
        
        
    }
    private void SpawnPlayer(int index)
    {
        Vector2Int spawnPoint = spawnPoints[index];
        Instantiate(player, new Vector3(spawnPoint.x, 1, spawnPoint.y), Quaternion.identity);
    }

 
}
