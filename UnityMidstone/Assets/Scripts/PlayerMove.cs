using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller; // Publicly define our character controller

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public Transform groundCheck; // Check if we are on the ground
    public float groundDistance = 0.18f;
    public LayerMask groundMask; // Set a field for ground layer
    float speed; // moving speed

    Vector3 velocity;
    public bool grounded;
    //private void Start()
    //{
    //    // always add a controller
    //    controller = gameObject.AddComponent<CharacterController>();
    //}
    private void Update() 
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Debug.Log("Player Position: " + groundCheck.position);
        Debug.Log("Player Velocity: " + velocity);

        if (grounded && velocity.y < 0) // If the player is grounded
        {
            velocity.y = -2f;
            Debug.Log("Grounded");
        }

        if (Input.GetKey(KeyCode.LeftShift)) // adjust move speed
        {
            speed = runSpeed; 
        }
        else
        {
            speed = walkSpeed;
        }

        float x = Input.GetAxis("Horizontal"); // Define  horizontal" axis
        float z = Input.GetAxis("Vertical"); // Define vertical" axis

        Vector3 move = transform.right * x + transform.forward * z; // Define the move vector

        controller.Move(move * speed * Time.deltaTime); // Move function

        if (Input.GetButtonDown("Jump") && grounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }

        velocity.y += gravity * Time.deltaTime; // Set the regular gravity

        controller.Move(velocity * Time.deltaTime); // Apply gravity to character controller
    }
}