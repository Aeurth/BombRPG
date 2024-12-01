using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    // Event that other scripts can subscribe to
    public static event Action OnGridManagerInitialized;
    public static GridManager Instance { get; private set; }
 

    public int gridSizeX = 10;
    public int gridSizeY = 10;

    private GridCell[,] grid;
    List<Vector2Int> usedPositionsForDestructibles;

    [Header("Spawnables Prefabs")]
    public GameObject destructibePrefab; //destructible object
    public GameObject bombPrefab; 

    [Header("PowerUp Prefabs")]
    [SerializeField] GameObject PwRadius;
    [SerializeField] GameObject PwmMaxBombs;
    [SerializeField] GameObject PwSpeed;


    
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
    private void Start()
    {
        grid = new GridCell[gridSizeX, gridSizeY];
        InitializeGrid();
        // Notify other scripts that GridManager is initialized
        OnGridManagerInitialized ?.Invoke();
        Debug.Log("GridManager Initialised");
    }
    private void InitializeGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y] = new GridCell();
            }
        }
    }
    public bool IsCellEmpty(int x, int y)
    {
        return grid[x, y].IsEmpty;
    }
    public GameObject PlaceBombAtPlayerPosition(Vector3 playerPosition)
    {
        int gridX = Mathf.RoundToInt(playerPosition.x);
        int gridY = Mathf.RoundToInt(playerPosition.z);

        if (IsCellEmpty(gridX, gridY))
        {
            Vector3 bombPosition = new Vector3(gridX, 0, gridY);
            GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
            grid[gridX, gridY].IsEmpty = false;
            grid[gridX, gridY].ContainsBomb = true;
            return bomb;
        }
        return null;
    }
    public void FillGridWithItems(int destructiblesCount, Vector2Int[] spawnPoints) 
    { 
        usedPositionsForDestructibles = new List<Vector2Int>();
        int placedItems = 0;

        while (placedItems < destructiblesCount)
        {
            // Generate random x and y coordinates
            int randomX = UnityEngine.Random.Range(0, gridSizeX);
            int randomY = UnityEngine.Random.Range(0, gridSizeY);
            Vector2Int randomPosition = new Vector2Int(randomX, randomY);

            // If the position is not used yet, place an item there
            if (!usedPositionsForDestructibles.Contains(randomPosition) && !spawnPoints.Contains(randomPosition))
            {
                // Instantiate the item prefab at the position
                Vector3 worldPosition = new Vector3(randomX, 0, randomY); // Adjust Y axis as needed for your game
                Instantiate(destructibePrefab, worldPosition, Quaternion.identity);

             // Add the item to the grid and the used positions set
                grid[randomX, randomY].IsEmpty = false;

                usedPositionsForDestructibles.Add(randomPosition);

                 placedItems++;
            }
        }
    }
    public void FillGridWithPowerUp( string powerType, int count)
    {
        if (count > usedPositionsForDestructibles.Count)
            Console.WriteLine($"filling powerup:{powerType} exceeds obscticles count: {usedPositionsForDestructibles.Count}");
        for (int i = 0; i < count; i++)
        {
            switch (powerType)
            {
                case "ExplosionRadius":
                    SpawnItemInRandomDesctructible(PwRadius);
                    break;
                case "BombCount":
                    SpawnItemInRandomDesctructible(PwmMaxBombs);
                    break;
                case "PlayerSpeed":
                    SpawnItemInRandomDesctructible(PwSpeed);
                    break;
            }
        }
        
    }
    public void ClearGridCell(Vector2Int position) 
    {
        if(grid[position.x, position.y].IsEmpty)
        {
            return;
        }

        grid[position.x, position.y].ClearCell();
    }
    public void ClearGridCell(Vector3 position)
    {
        int x = (int)Math.Ceiling(position.x);
        int y = (int)Math.Ceiling(position.z);

        if (grid[x, y].IsEmpty)
        {
            return;
        }

        grid[x, y].ClearCell();
    }
    private void SpawnItemInRandomDesctructible(GameObject ob)
    {
        int randomIndex = UnityEngine.Random.Range(0, usedPositionsForDestructibles.Count);
        Vector2Int position = usedPositionsForDestructibles[randomIndex];
  

        if (grid[position.x, position.y].ContainsItem == true)
        {
            SpawnItemInRandomDesctructible( ob );
        }
        else
        {
            Instantiate(ob, new Vector3(position.x, 1f, position.y), Quaternion.identity);
            grid[position.x, position.y].ContainsItem = true;

            usedPositionsForDestructibles.Remove(position);
            return;
        }
    }
    public void NewGrid(int x, int y)
    {
        gridSizeX = x; gridSizeY = y;
        grid = new GridCell[gridSizeX, gridSizeY];
        usedPositionsForDestructibles = new List<Vector2Int>();
        InitializeGrid();
        
    }
}
