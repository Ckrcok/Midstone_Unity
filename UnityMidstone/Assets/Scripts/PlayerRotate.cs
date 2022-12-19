using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRotate : MonoBehaviour
{
    // References to other scripts
    GameController gameController;
    Collectibles collectable;

    // Crosshair and mouse move setup
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

    // Stamina Item Interaction Setup
    [SerializeField]
    private RectTransform interactionPromt;    

    [SerializeField]
    private RectTransform interactionResult;

    private bool staminaItemRayHit = false;
    
    public static bool regenerateStamina = false;
    public static Coroutine staminaItemConsume; // end visual after 2 seconds  

    // HHawk Interaction Setup
    [SerializeField]
    private RectTransform interactionPromtHHawk;

    [SerializeField]
    private RectTransform interactionResultHHawk;

    [SerializeField]
    private RectTransform hHawkCollected0;

    [SerializeField]
    private RectTransform hHawkCollected1;

    [SerializeField]
    private RectTransform hHawkCollected2;

    [SerializeField]
    private RectTransform hHawkCollected3;

    [SerializeField]
    private RectTransform hHawksCollected;


    private bool hHawkRayHit = false;
    public static Coroutine hHawkCollected;
    public static Coroutine interactionDisabled;
    public static Coroutine allHHawksCollectedStart;
    public static Coroutine allHHawksCollectedFinish;
    public static Coroutine goToExit;



    private void Start() 
    {
        Cursor.visible = false; // hide the cursor duing gameplay
        Cursor.lockState = CursorLockMode.Locked; // lock cursor to center

        // Set collectible count to Game Controller
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameController.collectible = 0;  
        hHawkCollected0.gameObject.SetActive(true); // set initial sprite
    }

    private void Update() 
    {
        // Crosshair & Mouse Move

        // Get mouse inputs
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
        if(Physics.Raycast(transform.position, transform.forward, out hit, 20f)) //play with distance to see best outcome
        {
            if (hit.transform.gameObject.CompareTag("Enemy")) // UI layer tag Enemy should be applied
            {
                foreach (Image crosshairImage in crosshairImages)
                {
                    crosshairImage.color = new Color(1f, 0f, 0f, 1f);
                }
            }

            else if (hit.transform.gameObject.CompareTag("StaminaDrink"))
            {
                foreach (Image crosshairImage in crosshairImages)
                {
                    crosshairImage.color = new Color(0f, 1f, 0f, 1f);
                }

            }

            else if (hit.transform.gameObject.CompareTag("HHawkCollectible"))
            {
                foreach (Image crosshairImage in crosshairImages)
                {
                    crosshairImage.color = new Color(0f, 1f, 0f, 1f);
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

        StaminaItemRay();
        HHawkItemRay();

    }


    // Stamina Item Interaction routine

    // Raycast function to detect interactable object
    private void StaminaItemRay()
    {
        RaycastHit hitStamina;
        if (Physics.Raycast(transform.position, transform.forward, out hitStamina, 2f)) //play with distance to see best outcome
        {
            if (hitStamina.transform.gameObject.CompareTag("StaminaDrink"))
            {
                interactionPromt.gameObject.SetActive(true);   // show interaction prompt on screen
                staminaItemRayHit = true;
            }

        }
        else
        {
            interactionPromt.gameObject.SetActive(false);
            staminaItemRayHit = false;
        }

        if (staminaItemRayHit)
        {
            // TODO play audio cue

            // Key Down event
            if (Input.GetKeyDown(KeyCode.E))
            {
                StaminaConsume();
                // TODO add audio
            }

        }
    }

    // Consume function
    public void StaminaConsume()
    {
        interactionPromt.gameObject.SetActive(false); // should add delay or implement a coroutine to make it work as intended
        interactionResult.gameObject.SetActive(true);
        regenerateStamina = true;
        Debug.Log("Regen!");
        staminaItemConsume = StartCoroutine(RemoveOutcome());
    }

  
    public IEnumerator RemoveOutcome()
    {
        yield return new WaitForSeconds(3f);
        interactionResult.gameObject.SetActive(false);        
    }


    // Collectable Humber Hawk Interaction routine

    // Raycast function to detect interactable object

    private void HHawkItemRay()
    {
        RaycastHit hitHHawk;
        if (Physics.Raycast(transform.position, transform.forward, out hitHHawk, 2f)) //play with distance to see best outcome
        {
            if (hitHHawk.transform.gameObject.CompareTag("HHawkCollectible") && Collectibles.itemCollected == false)
            {
                interactionPromtHHawk.gameObject.SetActive(true);   // show interaction prompt on screen
                hHawkRayHit = true;
            }

        }
        else
        {
            interactionPromtHHawk.gameObject.SetActive(false);
            hHawkRayHit = false;
        }

        if (hHawkRayHit)
        {
            // TODO play audio cue

            // Key Down event
            if (Input.GetKeyDown(KeyCode.E))
            {
                CollectHHawk();
                // TODO add audio
            }

        }

    }

    // Collection function
    public void CollectHHawk()
    {
        interactionPromtHHawk.gameObject.SetActive(false); // should add delay or implement a coroutine to make it work as intended
        
        Debug.Log("Got One!");
        
        if(Collectibles.itemCollected == false)
        {
            gameController.collectible++;
            interactionResultHHawk.gameObject.SetActive(true);
            AddHawkToHud();
            Collectibles.itemCollected = true;
            hHawkCollected = StartCoroutine(OnCollectHHawk());
            interactionDisabled = StartCoroutine(DisableCollect());
        }
        // TODO
        // Play audio
    }
  

    public IEnumerator OnCollectHHawk()
    {
        yield return new WaitForSeconds(2f);
        interactionResultHHawk.gameObject.SetActive(false);
    }

    public IEnumerator DisableCollect()
    {
        yield return new WaitForSeconds(10f);
        Collectibles.itemCollected = false;
    }

    public IEnumerator AllHHawksConnectedStart()
    {
        yield return new WaitForSeconds(3f);
        hHawksCollected.gameObject.SetActive(true);
        allHHawksCollectedFinish = StartCoroutine(AllHHawksConnectedFinish());
    }

    public IEnumerator AllHHawksConnectedFinish()
    {
        yield return new WaitForSeconds(2f);
        hHawksCollected.gameObject.SetActive(false);
    }



    // Configure the sprite on HUD
    public void AddHawkToHud()
    {
        if(gameController.collectible == 0)
        {
            hHawkCollected0.gameObject.SetActive(true);
            hHawkCollected1.gameObject.SetActive(false);
            hHawkCollected2.gameObject.SetActive(false);
            hHawkCollected3.gameObject.SetActive(false);
        }
        else if(gameController.collectible == 1)
        {
            hHawkCollected0.gameObject.SetActive(false);
            hHawkCollected1.gameObject.SetActive(true);
            hHawkCollected2.gameObject.SetActive(false);
            hHawkCollected3.gameObject.SetActive(false);
        }
        else if(gameController.collectible == 2)
        {
            hHawkCollected0.gameObject.SetActive(false);
            hHawkCollected1.gameObject.SetActive(false);
            hHawkCollected2.gameObject.SetActive(true);
            hHawkCollected3.gameObject.SetActive(false);
        }
        else if(gameController.collectible == 3)
        {
            hHawkCollected0.gameObject.SetActive(false);
            hHawkCollected1.gameObject.SetActive(false);
            hHawkCollected2.gameObject.SetActive(false);
            hHawkCollected3.gameObject.SetActive(true);

            allHHawksCollectedStart = StartCoroutine(AllHHawksConnectedStart());
        }


    }    
    
    // UNFINISHED: 

    // SFX
    // GAME OVER SCREEN
    // ENEMY ENCOUNTER DETAILS
    // ANIMATED SPRITES
    // TOON SHADER
}
