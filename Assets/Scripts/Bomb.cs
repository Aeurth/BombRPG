using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    Collider objectCollider;

    private void Awake()
    {
        AnimationEvents.DetonateEvent += Explode;
    }
    private void Start()
    {
        objectCollider = GetComponent<Collider>();
        objectCollider.isTrigger = true; // set the bomb to be walkable through and reactive to triggers
    }
    public void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectCollider.isTrigger = false; // Become solid after the player leaves
        }
    }

}
