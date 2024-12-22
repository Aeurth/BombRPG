using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelNavMeshBaker : MonoBehaviour
{
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
    }
    void BakeNavMesh()
    {
        navMesh.BuildNavMesh();
        Debug.Log("NavMesh baked at runtime!");
    }


}
