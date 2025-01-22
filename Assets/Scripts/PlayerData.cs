using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public int health {  get; private set; }
    public Vector3 position { get; private set; }
    public PlayerData(int health, Vector3 position)
    {
        this.health = health;
        this.position = position;
    }


}