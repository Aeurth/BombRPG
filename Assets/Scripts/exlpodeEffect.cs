using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exlpodeEffect : MonoBehaviour
{
    public ParticleSystem system;

    // Start is called before the first frame update
    void Start()
    {
        timeout();
    }
    private void timeout()
    {
        Destroy(gameObject, system.main.duration);
    }
}
