using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlerNPC : MonoBehaviour
{
    static event Action targetReached;
    [SerializeField] float moveSpeed;
    [SerializeField] int detectionRange;
    private Coroutine movementCoroutine;
    private Rigidbody rb;

    private void Awake()
    {
        targetReached += OnTargetreached;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log($"NPC start pos{rb.position}");
        SetPath();
    }


    private void SetPath()
    {
        Vector2Int target = CheckForTarget();
        List<Vector2Int> path = GridManager.Instance.GetPath(GetCurrentPosition2D(), target);

        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine); // Stop any ongoing movement
        }

        if (path != null && path.Count > 1)
        {
            movementCoroutine = StartCoroutine(FollowPath(path));
        }
        else
        {
            Debug.Log("NPC does not have a path");
            return;
        }

        GridManager.OnGridManagerInitialized -= SetPath;
    }

    private IEnumerator FollowPath(List<Vector2Int> path)
    {
        foreach (var cell in path)
        {
            Vector3 targetPosition = GridToWorldPosition(cell);

            // Move toward the target position
            while (Vector3.Distance(rb.position, targetPosition) > 0.1f)
            {
                rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime));
                yield return null; // Wait for the next frame
            }

            yield return null; // Optional: Add a small pause at each step
        }
        Debug.Log("target reached!");
        targetReached?.Invoke();
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        // Convert grid coordinates to world coordinates
        return new Vector3(gridPosition.x, rb.position.y, gridPosition.y);
    }
    private void OnTargetreached()
    {
        SetPath();

    }
    private Vector2Int CheckForTarget()
    {
        Vector2Int target = GridManager.Instance.GetEmptyCellInRange(GetCurrentPosition2D(), detectionRange);
        Debug.Log($"NPC target:{target.x}, {target.y}");
        return target;
    }
    private Vector2Int GetCurrentPosition2D()
    {
        return new Vector2Int(Mathf.RoundToInt(rb.position.x), Mathf.RoundToInt(rb.position.z));
    }
}
