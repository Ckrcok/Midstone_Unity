using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private GameObject crosshair; 
    public Image[] crosshairImages; //crosshair images array
    public float regularFOV = 60f;
    public float aimingFOV = 50f;
    public float currentFOV = 60f;
    public float aimingSPeed = 10f;

    [SerializeField] public float rotationSensitivity = 750f; 
    public Transform playerBody; 
    float xRotation = 0f; // Define x rotation variable
    [SerializeField] private float clampDegreeUp = 70f; // look up limit
    [SerializeField] private float clampDegreeDown = -90f; // look down limit

    private void Start() 
    {
        Cursor.visible = false; // hide the cursor duing gameplay
        Cursor.lockState = CursorLockMode.Locked; // lock cursor to center
    }

    private void Update() 
    {
        // get mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime; 

        // Y input rotates on X-Axis (pitch)
        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, clampDegreeDown, clampDegreeUp); 

        // apply rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
        playerBody.Rotate(Vector3.up * mouseX); // X input rotates on Y-Axis (yaw)

        // change FOV according to crosshair state
        if (crosshair.GetComponent<Crosshair>().Aiming)
        {
            currentFOV = Mathf.Lerp(currentFOV, aimingFOV, Time.deltaTime * aimingSPeed);
        }
        else
        {
            currentFOV = Mathf.Lerp(currentFOV, regularFOV, Time.deltaTime * aimingSPeed);
        }

        this.gameObject.GetComponentInParent<Camera>().fieldOfView = currentFOV;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 200f)) //play with distance to see best outcome
        {
            if(hit.transform.gameObject.CompareTag("Enemy")) // UI layer tag Enemy should be applied
            {
                foreach(Image crosshairImage in crosshairImages)
                {
                    crosshairImage.color = new Color(1f, 0f, 0f, 1f);
                }
            }
            else
            {
                foreach (Image crosshairImage in crosshairImages)
                {
                    crosshairImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                }
            }
        }
        else
        {
            foreach (Image crosshairImage in crosshairImages)
            {
                crosshairImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            }
        }
    }
}
