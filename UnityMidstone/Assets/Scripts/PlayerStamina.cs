using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    // Stamina values
    public int maxStamina = 100;
    public int currentStamina = 100;

    // Stamina regeneration values
    public int staminaRegenCooldown = 5;
    public int staminaRegenIncrement = 1;
    public float staminaInecrementTime = 1;
    public Coroutine staminaRegen; // start regeneration

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
            DecreaseStamina(10);
        }
    }

    public void DecreaseStamina(int decreaseAmount)
    {
        currentStamina -= decreaseAmount;

        staminaBar.SetStamina(currentStamina);

        if (currentStamina <= 0)
            GiveUp();
        else if (staminaRegen != null)
            StopCoroutine(staminaRegen);

        staminaRegen = StartCoroutine(RegenerateStamina());

    }

    public void GiveUp()
    {
        // player gives up - game over
        currentStamina = 0;

        if (staminaRegen != null)
            StopCoroutine(RegenerateStamina());

        print("THEY GOT YOU!!!");

    }

    public IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(staminaRegenCooldown);
        WaitForSeconds cooldown = new WaitForSeconds(staminaRegenCooldown);

        while(currentStamina < maxStamina) //&& not in combat && not running
        {
            currentStamina += staminaRegenIncrement;

            if(currentStamina > maxStamina)
                currentStamina = maxStamina;
            yield return cooldown; 
        }

        staminaRegen = null;

    }
    // stamina 





    // stamina < 10 -> runspeed = walk speed
    // stamina <0 && enemy trigger = game over
}
