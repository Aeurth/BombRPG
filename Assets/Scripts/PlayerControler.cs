using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

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
    void Update()
    {
        HandleMovementDirection();
 
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
}

