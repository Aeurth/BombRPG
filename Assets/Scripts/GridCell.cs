using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public bool IsEmpty { get; set; } = true;
    public bool ContainsItem { get; set; } = false;
    public bool ContainsBomb { get; set; } = false;
    public bool ContainsPlayer { get; set; } = false;
    public bool IsSolid { get; set; } = false;

    public GridCell()
    {
        // Initially, the cell is empty
        IsEmpty = true;
    }

    // This method is useful for resetting the cell state
    public void ClearCell()
    {
        IsEmpty = true;
        ContainsItem = false;
        ContainsBomb = false;
        ContainsPlayer = false;
    }
    public void AddItem()
    {
        IsEmpty = false;
        ContainsItem = true;
    }
}

