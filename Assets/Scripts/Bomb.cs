using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;

    public void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    
}
