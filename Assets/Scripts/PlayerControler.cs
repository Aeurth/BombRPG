using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static event Action<PlayerData> PlayerDataChanged;
    public Rigidbody Rigidbody { get; private set; }
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    int health;
    [SerializeField] int maxHealth;


    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft = KeyCode.A;

    private Vector3 direction = Vector3.back;


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }
    private void Start()
    {
        health = maxHealth;
        PlayerDataChanged?.Invoke(GetData());
    }
    void Update()
    {
        HandleMovementDirection();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            TakeDamage();
        }
    }
    public void TakeDamage(int damage = 1)
    {
        if( health > 0 )
        {
            health -= damage;
            PlayerDataChanged?.Invoke(GetData());
            Debug.Log($"player health: {health}");
        }
       
    }
    public void Heal(int amount)
    {
        if (health == maxHealth)
        {
            return;
        }
        if (health + amount < maxHealth)
        {
            health += amount;
            PlayerDataChanged?.Invoke(GetData());
        }
        else
        {
            health = maxHealth;
            PlayerDataChanged?.Invoke(GetData());
        }
            
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
   
    private void HandleMovementDirection()
    {
        if (Input.GetKey(inputUp))
        {
            direction = Vector3.forward;
        }else if (Input.GetKey(inputDown))
        {
            direction = Vector3.back;
        }else if (Input.GetKey(inputRight))
        {
            direction = Vector3.right;
        }
        else if (Input.GetKey(inputLeft))
        {
            direction = Vector3.left;
        }
        else
        {
            direction = Vector3.zero;
        }

    }
    private void HandleMovement()
    {
        Vector3 position = Rigidbody.transform.position;
        Vector3 translation = direction * moveSpeed * Time.fixedDeltaTime;

        Rigidbody.MovePosition(position + translation);

        if (translation != Vector3.zero)
        {
            // Calculate the rotation to face the movement direction
            Quaternion toRotation = Quaternion.LookRotation(translation, Vector3.up);

            // Smoothly rotate the Rigidbody towards the movement direction
            Rigidbody.MoveRotation(Quaternion.Slerp(Rigidbody.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
    public void IncreaseSpeed(float value = 1)
    {
        moveSpeed += value;
    }
    private PlayerData GetData()
    {
        return new PlayerData(health, transform.position);
    }
}

