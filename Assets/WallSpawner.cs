using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab; // Prefab for the wall
    public float cellSize = 1f; // Size of each grid cell
    public int gridWidth = 11; // Width of the grid
    public int gridHeight = 11; // Height of the grid

    public void SpawnWallAlongGrid(bool horizontal, int position)
    {
        // Calculate the wall's position
        Vector3 wallPosition;
        Vector3 wallScale;

        if (horizontal)
        {
            // Horizontal wall along the width
            wallPosition = new Vector3(gridWidth * cellSize / 2f - cellSize / 2f, 1, position * cellSize);
            wallScale = new Vector3(gridWidth * cellSize, 1, cellSize); // Scale to grid width
        }
        else
        {
            // Vertical wall along the height
            wallPosition = new Vector3(position * cellSize, 1, gridHeight * cellSize / 2f - cellSize / 2f);
            wallScale = new Vector3(cellSize, 1, gridHeight * cellSize); // Scale to grid height
        }

        // Instantiate the wall
        GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);
        wall.transform.localScale = wallScale;

        // Optionally, tag the wall as "Obstacle"
        wall.tag = "Obstacle";
    }

    void Start()
    {
        // Example: Create a horizontal wall along the bottom row
        SpawnWallAlongGrid(horizontal: true, position: -1);

        // Example: Create a vertical wall along the left column
        SpawnWallAlongGrid(horizontal: false, position: -1);
        // Example: Create a horizontal wall along the top row
        SpawnWallAlongGrid(horizontal: true, gridHeight);

        // Example: Create a vertical wall along the left column
        SpawnWallAlongGrid(horizontal: false, gridWidth);
    }
}

