using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class exlpodeEffect : MonoBehaviour
{
    public ParticleSystem system;
    private BoxCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        AddCollider();
        timeout();
    }
    private void timeout()
    {
        Destroy(gameObject, system.main.duration);
    }
    private void AddCollider()
    {
        collider = this.AddComponent<BoxCollider>();
        collider.tag = "Explosion";
        collider.isTrigger = true;
        float size = 0.8f;
        collider.size = new Vector3(size, size, size);
    }
}
