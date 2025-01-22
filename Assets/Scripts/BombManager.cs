using System;
using UnityEngine;


public struct BombsData
{
    public int maxBombsCount { get; private set; }
    public int bombsCount { get; private set; }
    public int explotionRange { get; private set; }

    public BombsData(int maxBombsCount, int bombsCount, int explotionRange)
    {
        this.maxBombsCount = maxBombsCount;
        this.bombsCount = bombsCount;
        this.explotionRange = explotionRange;
    }
}


//todo: refactor this class into player controler and the bomb itself, probably use raycasts insted of collision checking
public class BombManager : MonoBehaviour
{
    public static event Action<BombsData> OnBombsDataChanged;
    public static event Action OnDestructibleDestroyed;

    // in this game I believe there should not be more than 5 bombs on the player so array size will be 5,
    // if there should be more bombs - increse the array size in array intialization in on start function
    private GameObject[] BOMBS; 
    public int maxBombs = 1;
    public int bombsCount = 0;
    private int explosionRange = 1;
    int explosionLayerMask;

    [Header("Prefabs")]
    public GameObject explosionEffect;

    
    private void Start()
    {
        explosionLayerMask = LayerMask.GetMask("Explosion");
        BOMBS = new GameObject[5];
    }
    private void OnEnable()
    {
        UIManager.OnUIManagerInitialised += HandleUI;
        AnimationEvents.DetonateEvent += DetonateBombs;
    }
    void Update()
    {
        HandleBombPlacement();
    }
    private void HandleBombPlacement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bombsCount < maxBombs)
            {
                PlaceBomb();
                OnBombsDataChanged?.Invoke(GetCurrentData());
            }
        }
    }
    private void PlaceBomb()
    {
        if (GridManager.Instance != null)
        {
            GameObject bomb = GridManager.Instance.PlaceBombAtPlayerPosition(transform.position);
            if (bomb != null)
            {
                BOMBS[bombsCount] = bomb;
                bombsCount++;
                OnBombsDataChanged?.Invoke(GetCurrentData());
            }
        }
        else
        {
            Debug.Log("grid instance is null");
        }
    }
    private Vector3 GetBombPosition(int index)
    {
        Vector3 bombPosition = BOMBS[index].transform.position;
        return bombPosition;
    }
    private void DetonateBombs()
    {
        //detonate bombs
        for (int i = 0; i < bombsCount; i++)
        {
            Vector3 position = GetBombPosition(i);

            ExplodeToDirection(Vector3.forward, position, 0);
            ExplodeToDirection(Vector3.back, position, 0);
            ExplodeToDirection(Vector3.right, position, 0);
            ExplodeToDirection(Vector3.left, position, 0);
            BOMBS[i].GetComponent<Bomb>().Explode();
            
        }
        //clear bombs array
        ClearBombsArray();
    }
    private void ClearBombsArray()
    {
        for (int i = 0; i < bombsCount; i++)
        {
            Vector3 bombPos = BOMBS[i].transform.position;
            GridManager.Instance.ClearGridCell(bombPos);
            BOMBS[i] = null;
            
        }
        bombsCount = 0;
        OnBombsDataChanged?.Invoke(GetCurrentData());
    }
    private void ExplodeToDirection(Vector3 direction, Vector3 currentPosition, int range)
    {
        
        //create position where object collision will be tested
        Vector3 position = currentPosition + direction;
        Vector2 position2D = new Vector2(position.x, position.z);

        // check if target position is in grid bounds
        Rect gridBounds = new Rect(1, 1, GridManager.Instance.gridSizeX - 1, GridManager.Instance.gridSizeY - 1);
       // if (gridBounds.Contains(position2D))
          //  Debug.Log($"Position: {position2D}");

        

        if (range < explosionRange)
        {
            //creating a collider in the target position to check if occupied by an object from explosion layer
            Collider[] colliders = Physics.OverlapBox(position, Vector3.one / 2f, Quaternion.identity, explosionLayerMask);


            //calls self recursively until finds an object
            if (colliders.Length == 0)
            { 

                Instantiate(explosionEffect, new Vector3(position.x, 1/2f, position.z), Quaternion.identity);

                int updatedRange = range + 1;
                ExplodeToDirection(direction, position, updatedRange);
            }
            else
            {// destroys object at the end of recursive function if there is an object to be destroyed
                if (colliders[0].CompareTag("Destructible"))
                {
                    GameObject destructible = colliders[0].gameObject;
                    GridManager.Instance.ClearGridCell(destructible.transform.position);
                    Destroy(destructible);
                    OnDestructibleDestroyed?.Invoke();
                }
                else if (colliders[0].CompareTag("Player"))
                {
                    Instantiate(explosionEffect, new Vector3(position.x, 1 / 2f, position.z), Quaternion.identity);

                    int updatedRange = range + 1;
                    ExplodeToDirection(direction, position, updatedRange);
                }
                

            }
        }
        
        return;
    }
    
    private BombsData GetCurrentData()
    {
        return new BombsData(maxBombs, bombsCount, explosionRange);
    }
    public void IncreaseExplotionRange(int value = 1)
    {
        explosionRange += value;
        OnBombsDataChanged?.Invoke(GetCurrentData());
    }
    public void IncreaseMaxBombsCount(int value = 1)
    {
        maxBombs += value;
        OnBombsDataChanged?.Invoke(GetCurrentData());
    }

    private void HandleUI()
    {
        OnBombsDataChanged?.Invoke(GetCurrentData());
    }
}
