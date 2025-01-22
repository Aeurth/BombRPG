using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static Action DetonateEvent;

    public void Detonate(int flag)
    {
        if (flag == 1) {
            DetonateEvent?.Invoke();

        }
        Debug.Log("explosion event!");
    }
}
