using System;
using UnityEngine;
using UnityEngine.Playables;


public class PlayerController : MonoBehaviour
{
    public static event Action<PlayerData> PlayerDataChanged;
    public Rigidbody Rigidbody { get; private set; }
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    int health;
    int maxHealth;
    int bombsCount = 0;

    Player player;


    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft = KeyCode.A;

    private Vector3 direction = Vector3.back;

    private void Start()
    {

        Rigidbody = this.GetComponent<Rigidbody>();
        player = this.GetComponent<Player>();
        player.Initialize(new IdleState());
        health = maxHealth;
        PlayerDataChanged?.Invoke(GetData());

    }
    void Update()
    {
        HandleMovementDirection();
        HandlePlayerInputs();
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
            
        }
        else if (Input.GetKey(inputDown))
        {
            direction = Vector3.back;
            
        }
        else if (Input.GetKey(inputRight))
        {
            direction = Vector3.right;
            
        }
        else if (Input.GetKey(inputLeft))
        {
            direction = Vector3.left;
          
        }
        else if(!Input.anyKey)
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
    private void HandlePlayerInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            player.PlayAnimation("Dummy_root_Dummy_root_Pickup");
        }
        if(Input.GetMouseButtonDown(0))
        {
            //detonate bombs animation if there is any
            if( bombsCount  > 0 )
            {
                //invoke event for each bomb to explode
                player.PlayAnimation("Dummy_root_Dummy_root_Joy");
            }
            
        }

        if(direction != Vector3.zero)
        {
            player.SetAnimationParam(PlayerInputs.IsWalking, true);
        }
        else
        {
            player.SetAnimationParam(PlayerInputs.IsWalking, false);
        }
    }
    private PlayerData GetData()
    {
        return new PlayerData(health, transform.position);
    }
}

