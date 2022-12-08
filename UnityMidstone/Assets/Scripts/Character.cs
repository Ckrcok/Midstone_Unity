using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour {
    public Rigidbody rb;

    // Used to control "Character" movement
    float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpSpeed;

    float moveForward;
    float moveSide;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckDistance;

    // Used for Character animations
    Animator animator;

    // Start is called before the first frame update
    void Start() {
        name = "Character";

        rb =GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        animator = GetComponent<Animator>();
        if (animator != null ) {
            animator.applyRootMotion = false;
    
        }

        if(walkSpeed <= 0 ) {
            walkSpeed = 5.0f;
            Debug.Log(name + "walk speed not set. Defaulting to " + walkSpeed);
        }

        if (sprintSpeed <= 0) {
            sprintSpeed = 10.0f;
            Debug.Log(name + "walk speed not set. Defaulting to " + sprintSpeed);
        }

        if (jumpSpeed <= 0) {
            jumpSpeed = 8.0f;
            Debug.Log(name + "jump speed not set. Defaulting to " + jumpSpeed);
        }

        if (!groundCheck) {
            Debug.LogError(name + ": Missing groundCheck");
        }

        if (groundCheckDistance <= 0) {
            groundCheckDistance = 0.3f;
            Debug.Log(name + ": groundCheckDistance not set. Defaulting to " + groundCheckDistance);
        }

    }

    // Update is called once per frame
    void Update() {

        if (groundCheck) {
            isGrounded = Physics.Raycast(groundCheck.position, -groundCheck.up, groundCheckDistance);

            Debug.DrawRay(groundCheck.position, -groundCheck.up * groundCheckDistance, Color.red);
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (currentSpeed != sprintSpeed)
                currentSpeed = sprintSpeed;
        }
        else
            currentSpeed = walkSpeed;

        moveForward = Input.GetAxis("Vertical") * currentSpeed;
        moveSide = Input.GetAxis("Horizontal") * currentSpeed;

        if (isGrounded && Input.GetButtonDown("Jump")) {

            jump();
        }

        // Use P to instakill "character"
        if (Input.GetKeyDown(KeyCode.P)) {
            // Call "die" function created below
            die();
        }


    }
    void FixedUpdate() {

        rb.velocity = (transform.forward * moveForward) + (transform.right * moveSide) + (transform.up * rb.velocity.y);

        animator.SetBool("isGrounded", isGrounded);

        animator.SetFloat("Speed", transform.InverseTransformDirection(rb.velocity).z);

    }
    void jump() {
        Debug.Log("Jumping");

        rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

    //void fire() {
    //    Debug.Log("Pew Pew");

    //    animator.SetTrigger("Attack");


    //    Rigidbody.temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

    //    temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);

    //    Destroy(temp.gameObject, 2.0f);
    //}

    void die() {
        Debug.Log("Charater Die");
    }


    // Using Rules:
    // - Both GameObjects in Scene need to have Colliders
    // - One or Both GameObjects need a Rigidbody
    // - Called once when collision starts 
    void OnCollisionEnter(Collision collision) {
        Debug.Log("OnCollisionEnter: " + collision.gameObject.name);
        Debug.Log("OnCollisionEnter: " + collision.gameObject.tag);

    }

    // Using Rules:
    // - Both GameObjects in Scene need to have Colliders
    // - One or Both GameObjects need a Rigidbody
    // - Called once as long as collision happens
    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay: " + collision.gameObject.name);
        Debug.Log("OnCollisionStay: " + collision.gameObject.tag);

    }

    // Using Rules:
    // - Both GameObjects in Scene need to have Colliders
    // - One or Both GameObjects need a Rigidbody
    // - Called once when collision ends
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit: " + collision.gameObject.name);
        Debug.Log("OnCollisionExit: " + collision.gameObject.tag);

    }
}
