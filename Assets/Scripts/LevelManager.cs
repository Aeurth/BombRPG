using System;
using UnityEngine;

//TODO: see if possible to make gridManager local object to the levelManager ( other Grid manager dependancies are: BombManager)
// otherwise change to event based system if its not too hard
//TODO: seperate this class logic between LevelManager and Level calsses
public class LevelManager: MonoBehaviour
{
    
    public static event Action<LevelData> OnLevelDataChanged;
    public static event Action OnConfigsSet;
    public static event Action levelComplete;
    public static event Action LevelLoaded;

    LevelConfig[] LevelConfigs;
    LevelConfig currentLevel;
    private int currentLevelIndex;
    Vector2Int[] spawnPoints;
    bool isConfigsSet = false;
    int DestructablesCount;

    [Header("prefabs")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject NPC_Prefab;
    [SerializeField] GameObject Tile;
    private GameObject player;

    private void OnEnable()
    {
        // Subscribe to events
        GridManager.OnGridManagerInitialized += HandleGridManagerInitialized;
        UIManager.OnUIManagerInitialised += HandleUIOnStart;
        BombManager.OnDestructibleDestroyed += HandleDestuctibleEvent;
        levelComplete += OnLevelComple;
        InputManager.nextClicked += LoadLevel;
        

    }
    private void Start()
    {
        Debug.Log("LevelManager initialised");
        LoadAllLevelsConfigs();
        LoadLevel();
    }
    private void LoadAllLevelsConfigs()
    {
        LevelConfigs = Resources.LoadAll<LevelConfig>("LevelConfigs");

    }
    private void SetUpLevelConfigs()
    {
        currentLevel = LevelConfigs[currentLevelIndex];
        DestructablesCount = currentLevel.DestructiblesCount;
        spawnPoints = currentLevel.SpawnPoints;
        isConfigsSet= true;
        OnConfigsSet?.Invoke();
        Debug.Log("Level configs are setup");
    }
    private void SaveLevelIndex(int levelIndex)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
    }
    private int LoadLevelIndex()
    {
        return PlayerPrefs.HasKey("CurrentLevel") ? PlayerPrefs.GetInt("CurrentLevel") : 0;
    }
    private void HandleGridManagerInitialized()
    {
        //perform actions that depend on level configs
        if (isConfigsSet)
        {
            GridManager.Instance.NewGrid(currentLevel.gridSizeX, currentLevel.gridSizeY);
            FillLevelWithItems();
            FillLevelWithPowerUps();
            // Unsubscribe from the event after handling the Grid
            OnConfigsSet -= HandleGridManagerInitialized;
        }
        else // Subscribe to the event only if configs aren't loaded yet
        {
            OnConfigsSet -= HandleGridManagerInitialized; // Ensure it's not subscribed multiple times
            OnConfigsSet += HandleGridManagerInitialized;
        }
        
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

        if (player == null)
        {
            Console.WriteLine($"{player} bitch");
            player = Instantiate(playerPrefab, new Vector3(spawnPoint.x, 1, spawnPoint.y), Quaternion.identity);
            player.SetActive(true);
        }
        else
        {
            player.transform.position = new Vector3(spawnPoint.x, 1, spawnPoint.y);
            player.SetActive(true);
        }
        
    }
    private void SpawnNPC()
    {
        float randomX = UnityEngine.Random.Range(0, currentLevel.gridSizeX);
        float randomY = UnityEngine.Random.Range(0, currentLevel.gridSizeY);
        Instantiate(NPC_Prefab, new Vector3(randomX, 1, randomY), Quaternion.identity);
    }
    private void OnLevelComple()
    {
        currentLevelIndex++;
        SaveLevelIndex(currentLevelIndex);
        ClearLevel();
    }
    private void SetTileSize()
    {
        SpriteRenderer renderer = Tile.GetComponent<SpriteRenderer>();
        renderer.size = new Vector2(currentLevel.gridSizeX, currentLevel.gridSizeY);
        Tile.SetActive(true);
    }
    private void SetTileColiderSize()
    {
        BoxCollider collider = Tile.GetComponent<BoxCollider>();
        collider.size = new Vector3(currentLevel.gridSizeX, currentLevel.gridSizeY, 0.2f);
        collider.center = new Vector3(currentLevel.gridSizeX / 2, currentLevel.gridSizeY / 2, 0);

    }
    private void ClearLevel()
    {
        player.SetActive(false);
    }
    private void LoadLevel()
    {
        currentLevelIndex = LoadLevelIndex();
        SetUpLevelConfigs();
        SetTileSize();
        SpawnPlayer(0);
        LevelLoaded?.Invoke();
        SpawnNPC();


    }
 
}
