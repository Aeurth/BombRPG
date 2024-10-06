using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class BombManager : MonoBehaviour
{
    
    public static event Action OnDestructibleDestroyed;
    public static event Action<int> OnBombsCountChanged;
    // in this game I believe there should not be more than 5 bombs on the player so array size will be 5,
    // if there should be more bombs - increse the array size in array intialization in start function
    private GameObject[] BOMBS; 
    public int max_bombs = 1;
    private int bomb_count = 0;
    int explosionLayerMask;

    public int explosionRange = 1;
    public GameObject explosionEffect;

    
    private void Start()
    {
        explosionLayerMask = LayerMask.GetMask("Explosion");
        BOMBS = new GameObject[5];
    }
    private void OnEnable()
    {
        UIManager.OnGridUIManagerInitialised += HandleUI;
    }
    void Update()
    {
        HandleBombPlacement();
        HandleBombDetonate();
    }
    private void HandleBombPlacement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bomb_count < max_bombs)
            {
                PlaceBomb();
            }
        }
    }
    private void HandleBombDetonate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetonateBombs();
        }
    }
    private void PlaceBomb()
    {
        if (GridManager.Instance != null)
        {
            GameObject bomb = GridManager.Instance.PlaceBombAtPlayerPosition(transform.position);
            if (bomb != null)
            {
                BOMBS[bomb_count] = bomb;
                bomb_count++;
                OnBombsCountChanged?.Invoke(bomb_count);
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
        for (int i = 0; i < bomb_count; i++)
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
        for (int i = 0; i < bomb_count; i++)
        {
            Vector3 bombPos = BOMBS[i].transform.position;
            GridManager.Instance.ClearGridCell(bombPos);
            BOMBS[i] = null;
            
        }
        bomb_count = 0;
        OnBombsCountChanged.Invoke(bomb_count);
    }

    private void ExplodeToDirection(Vector3 direction, Vector3 currentPosition, int range)
    {
        
        // check if target position is in bounds of 10x10 grid
        Vector3 position = currentPosition + direction;

        if (position.x == 10 || position.z == 10)
            return;
        if (position.x < 0 || position.z < 0)
            return;

        

        if (range < explosionRange)
        {
            //creating a collider in the target position to check if occupied by an object from explosion layer
            Collider[] colliders = Physics.OverlapBox(position, Vector3.one / 2f, Quaternion.identity, explosionLayerMask);

            //calls self recursively until finds an object
            if (colliders.Length == 0)
            { 

                Instantiate(explosionEffect, new Vector3(position.x, 0, position.z), Quaternion.identity);
                int updatedRange = range + 1;
                ExplodeToDirection(direction, position, updatedRange);
            }
            else
            {// destroys object at the end of recursive function if there is an object to be destroyed
                for (int i = 0; i < colliders.Length; i++)
                {
                    GameObject destructible = colliders[i].gameObject;
                    GridManager.Instance.ClearGridCell(destructible.transform.position);
                    Destroy(destructible);
                    OnDestructibleDestroyed?.Invoke();
                }
            }
        }
        
        return;
    }

    private void HandleUI()
    {
        UIManager.Instance.UpdateMaxBombsCount(max_bombs);
        UIManager.Instance.UpdateCount(bomb_count);
    }
}
