using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Vector2Int position {  get; private set; }
    public bool IsEmpty { get; set; }
    public bool ContainsItem { get; set; }
    public GridCell()
    {
        IsEmpty = true;
        ContainsItem = false;
        position = new Vector2Int();
    }
    public GridCell(Vector2Int position)
    {
        IsEmpty = true;
        ContainsItem = false;
        this.position = position;
    }
    public void ClearCell()
    {
        IsEmpty = true;
        ContainsItem = false;
    }
    public void AddItem()
    {
        IsEmpty = false;
        ContainsItem = true;

    }
}

