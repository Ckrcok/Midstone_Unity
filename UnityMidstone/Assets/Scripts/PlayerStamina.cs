using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public int maxStamina = 100;
    public int currentStamina = 100;

    public StaminaBar staminaBar;
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            GetNumb(10);
        }
    }

    void GetNumb(int stun)
    {
        currentStamina -= stun;

        staminaBar.SetStamina(currentStamina);
    }
}
