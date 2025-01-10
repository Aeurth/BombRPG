using System.Collections.Generic;
using UnityEngine;


public class Pathfinding
{
    public static List<Vector2Int> FindPathBFS(GridCell[,] grid, Vector2Int start, Vector2Int end)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Helper function to check if a position is within bounds
        bool IsInBounds(Vector2Int position) =>
            position.x >= 0 && position.x < rows && position.y >= 0 && position.y < cols;

        // Helper function to get neighbors
        List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            var neighbors = new List<Vector2Int>();
            var directions = new List<Vector2Int>
            {
                new Vector2Int(0, 1), // Up
                new Vector2Int(1, 0), // Right
                new Vector2Int(0, -1), // Down
                new Vector2Int(-1, 0) // Left
            };

            foreach (var dir in directions)
            {
                var neighborPosition = position + dir;
                if (IsInBounds(neighborPosition) && grid[neighborPosition.x, neighborPosition.y].IsEmpty)
                {
                    neighbors.Add(neighborPosition);
                }
            }
            return neighbors;
        }

        // BFS initialization
        var queue = new Queue<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var visited = new HashSet<Vector2Int>();

        if (!IsInBounds(start) || !IsInBounds(end))
        {
            Debug.Log("Patfinding failed: out of bounds");
            return new List<Vector2Int>(); // No valid path if start or end is invalid
        }
            

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // Check if we've reached the end
            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            // Explore neighbors
            foreach (var neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current; // Record the path
                }
            }
        }
        Debug.Log("No Path found");// No path found
        return new List<Vector2Int>();
       
    }

    private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var path = new List<Vector2Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(current);
        path.Reverse();
        return path;
    }
}

