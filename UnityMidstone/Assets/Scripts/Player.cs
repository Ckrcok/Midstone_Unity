using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Character Controller
    public CharacterController controller; // Publicly define our character controller    

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 0.5f;
    public Transform groundCheck; // Check if we are on the ground
    public float groundDistance = 0.18f;
    public LayerMask groundMask; // Set a field for ground layer
    float speed;
    public static bool isRunning = false;

    Vector3 velocity;
    public bool grounded;

    // Stamina Bar
    public static float maxStamina = 100f;
    public static float currentStamina = 100f;
    public StaminaBar staminaBar;
    public static bool maxedStamina;

    // Stamina regeneration values
    public float staminaRegenCooldown = 10;
    public float staminaRegenIncrement = 1;
    public float staminaInecrementTime = 1;
    public static Coroutine staminaRegen; // start regeneration   

    // HumberHawk collectable setup
    public int collectedHHawk = 0;
    public int maxCollectableHHawk = 3;
    public bool isAllHawksCollected = false; // acts as key for the last door for Humber Hawk to help you escape

    // Enemy interaction setup
    public bool isBeingAttacked = false;
    public float attackDistance; // can be populated for different enemies


    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Debug.Log("Player Position: " + groundCheck.position);
        Debug.Log("Player Velocity: " + velocity);

        if (grounded && velocity.y < 0) // constant negative y velocity when grounded to provide realistic downward movement on slopes
        {
            velocity.y = -2f;
            Debug.Log("Grounded");
        }

        if (Input.GetKey(KeyCode.LeftShift)) // adjust move speed
        {
            speed = runSpeed;
            isRunning = true;
        }
        else
        {
            speed = walkSpeed;
            isRunning = false;
        }

        float x = Input.GetAxis("Horizontal"); // Define  horizontal" axis
        float z = Input.GetAxis("Vertical"); // Define vertical" axis

        Vector3 move = transform.right * x + transform.forward * z; // Define the move vector

        controller.Move(move * speed * Time.deltaTime); // Move function

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; // set Vertical velocity bound to gravity

        controller.Move(velocity * Time.deltaTime); // Apply Vertical velocity to character controller

        if (Input.GetKeyDown(KeyCode.K))
        {
            DecreaseStamina(10);
        }

        if (PlayerRotate.regenerateStamina == true)
        {
            staminaRegen = StartCoroutine(RegenerateStamina(0.1f, 0.1f));
        }

        if (currentStamina == maxStamina)
        {
            maxedStamina = true;
            PlayerRotate.regenerateStamina = false;
        }


    
    }

    // Stamina Functions
    public void DecreaseStamina(int decreaseAmount)
    {
        currentStamina -= decreaseAmount;

        staminaBar.SetStamina(currentStamina);

        if (currentStamina <= 0)
            GiveUp();
        else if (staminaRegen != null)
            StopCoroutine(staminaRegen);

        staminaRegen = StartCoroutine(RegenerateStamina(1f, 1f));

    }

    public void GiveUp()
    {
        // player gives up - game over
        currentStamina = 0;

        if (staminaRegen != null)
            StopCoroutine(RegenerateStamina(1f, 1f));

        print("THEY GOT YOU!!!");

    }

    public IEnumerator RegenerateStamina(float cooldownMultiplier, float incrementMultiplier)
    {
        yield return new WaitForSeconds(staminaRegenCooldown * cooldownMultiplier);
        WaitForSeconds cooldown = new WaitForSeconds(staminaInecrementTime);

        while (currentStamina < maxStamina && !isRunning) //&& not in combat && not running
        {
            currentStamina += staminaRegenIncrement * incrementMultiplier;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            staminaBar.SetStamina(currentStamina);            

            yield return cooldown;
        }

        staminaRegen = null;

    }


    // Collectable Functions

    public void CollectHawk()
    {
        collectedHHawk++;

        if(collectedHHawk == maxCollectableHHawk)
        {
            isAllHawksCollected = true;
        }
        else isAllHawksCollected = false;
    }

    // Enemy attack

    public void BeingAttacked()
    {
        DecreaseStamina(5);
    }

}