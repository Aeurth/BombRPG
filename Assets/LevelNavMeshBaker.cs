using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelNavMeshBaker : MonoBehaviour
{
    event Action NavMeshInit;
    [SerializeField] GameObject floor;

    NavMeshSurface navMesh;

    private void Awake()
    {
        LevelManager.LevelLoaded += BakeNavMesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        navMesh = floor.GetComponent<NavMeshSurface>();
        NavMeshInit?.Invoke();
    }
    void BakeNavMesh()
    {
        //subscribe
        if(navMesh == null)
        {
            NavMeshInit += BakeNavMesh;
            Debug.Log("navMesh is null");
        }
        //unsubcribe
        else
        {
            navMesh.BuildNavMesh();
            Debug.Log("NavMesh baked at runtime!");
            NavMeshInit -= BakeNavMesh;
        }

        
    }


}
