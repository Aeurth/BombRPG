using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public struct SearchBounds
{
    public int right { get; private set; }
    public int left { get; private set; }
    public int top { get; private set; }
    public int bottom { get; private set; }

    public SearchBounds(Vector2Int center, int range, int gridSizeX, int gridSizeY)
    {
        right = center.x + range;
        if (right >= gridSizeX)
            right = gridSizeX - 1; // Adjust to stay within bounds

        left = center.x - range;
        if (left < 0)
            left = 0;

        top = center.y + range;
        if (top >= gridSizeY)
            top = gridSizeY - 1; // Adjust to stay within bounds

        bottom = center.y - range;
        if (bottom < 0)
            bottom = 0;
    }
    public SearchBounds(int left, int bottom, int right, int top )
    {
        this.right = right;
        this.left = left;
        this.top = top;
        this.bottom = bottom;
    }
}


//TODO: seperate path finding methods to pathfinding class and get Grid object by value each time for calculating path
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
                grid[x, y] = new GridCell(new Vector2Int(x, y));
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
            Vector3 bombPosition = new Vector3(gridX, 1/2f, gridY);
            GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
            grid[gridX, gridY].IsEmpty = false;
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
                Vector3 worldPosition = new Vector3(randomX, 0.6f, randomY); // Adjust Y axis as needed for your game
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
    public List<Vector2Int> GetPath(Vector2Int currentPosition, Vector2Int target)
    {
        if(target.x < 0)
        {
            Debug.Log("recieved negative target values in: Grid.GetPath()");
            return null;
        }
        List<Vector2Int> path = Pathfinding.FindPathBFS(grid, currentPosition, target);
        return path;
    }
    public Vector2Int GetEmptyCellInRange(Vector2Int position, int range)
    {
        SearchBounds bounds = new SearchBounds(position, range, gridSizeX, gridSizeY);

        Vector2Int cell = SearchBox(bounds, position);

        return cell;
    }
    private Vector2Int SearchBox(SearchBounds bounds, Vector2Int center)
    {
        // Iterate through the grid within the defined bounds
        for (int i = bounds.left; i <= bounds.right; i++)
        {
            for (int j = bounds.bottom; j <= bounds.top; j++)
            {
                // Check if the cell is empty
                if (grid[i, j].IsEmpty & new Vector2Int(i, j) != center)
                {
                    return new Vector2Int(i, j); // Return the first empty cell found
                }
            }
        }

        return new Vector2Int(-1, -1);//indicates failure
    }private Vector2Int SearchByQuadrant(SearchBounds bounds, Vector2Int center)
    {
        Vector2Int cell = new Vector2Int(-1, -1);

        SearchBounds Q1 = new SearchBounds(bounds.left, bounds.bottom, center.x, center.y);//left bottom
        SearchBounds Q2 = new SearchBounds(bounds.left, center.y, center.x, bounds.top);//left top
        SearchBounds Q3 = new SearchBounds(center.x, center.y, bounds.right, bounds.top);// right top
        SearchBounds Q4 = new SearchBounds(center.x, bounds.bottom, bounds.right, center.y);//right bottom


        //set the starting quadrant
        int quadrant = UnityEngine.Random.Range(0, 4);
        int checkedCount = 0;

        while (checkedCount < 4)
        {
            if (quadrant == 0)
            {
                cell = SearchBox(Q1, center);
                if (cell != new Vector2Int(-1, -1))
                    return cell;

                checkedCount++;
                if (quadrant == 3)
                    quadrant = 0;
                else
                    quadrant++;
            }
            if (quadrant == 1)
            {
                cell = SearchBox(Q2, center);
                if (cell != new Vector2Int(-1, -1))
                    return cell;
                checkedCount++;
                if (quadrant == 3)
                    quadrant = 0;
                else
                    quadrant++;
            }
            if (quadrant == 2)
            {
                cell = SearchBox(Q3, center);
                if (cell != new Vector2Int(-1, -1))
                        return cell;
                checkedCount++;
                if (quadrant == 3)
                    quadrant = 0;
                else
                    quadrant++;
            }
            if (quadrant == 3)
            {
                cell = SearchBox(Q4, center);
                if (cell != new Vector2Int(-1, -1))
                    return cell;
                checkedCount++;
                if (quadrant == 3)
                    quadrant = 0;
                else
                    quadrant++;
            }
        
        }

        

        Debug.Log("Search by quadrant did not find a target, returned negative value");
        return cell;
    }

    
}
