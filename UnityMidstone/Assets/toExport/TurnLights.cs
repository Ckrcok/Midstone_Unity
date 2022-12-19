using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLights : MonoBehaviour
{
    public GameObject light;
    private bool on = false;
    private KeyCode ActivatePower = KeyCode.Mouse0;

    void OnTriggerStay(Collider plyr)
    {
        if(plyr.tag == "Player" && Input.GetKeyDown("e") && !on)
        {
            light.SetActive(true);
            on = true;
        }else if (plyr.tag == "Player" && Input.GetKeyDown("e") && on)
        {
            light.SetActive(false);
            on = false;
        }
    }
}


