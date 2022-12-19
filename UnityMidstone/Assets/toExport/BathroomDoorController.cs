using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomDoorController : MonoBehaviour
{
    private Animator doorAnim;
    private bool doorOpen = false;
    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            doorAnim.Play("bathroomDoorOPen", 0, 0.0f);
            doorOpen = true;
        }

        else
        {
            doorAnim.Play("bathroomDoorClosed", 0, 0.0f);
            doorOpen = false;
        }
    }
}

