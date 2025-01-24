using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static Action<int> DetonateEvent;
    public static Action<Vector3> PlaceBombEvent;
    Player player;

    private void Start()
    {
        
       player = this.GetComponentInParent<Player>();
        Debug.Log(player.ToString());
    }
    public void Detonate()
    {
        if(player != null)
        {
            Debug.Log("sending animationEvent detonate ");
            DetonateEvent?.Invoke(player.ExplosionRange);
        }
        

        Debug.Log("explosion event!");
    }
    public void PlaceBomb()
    {
        PlaceBombEvent?.Invoke(gameObject.transform.position);
    }
}
