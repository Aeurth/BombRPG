using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public int health {  get; private set; }
    public PlayerData(int health)
    {
        this.health = health;
    }

}