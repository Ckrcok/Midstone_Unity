using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform crosshair;

    public GameObject player;

    // crosshair size
    public float size = 50f;
    public float aimed = 25f;
    public float idle = 50f;
    public float walk = 75f;
    public float run = 125f;
    public float jump = 125f;
    public float speed = 10f;


    private void Update()
    {
        if (Aiming)
        {
            size = Mathf.Lerp(size, aimed, Time.deltaTime * speed);
        }
        else if (Walking)
        {
            size = Mathf .Lerp(size, walk, Time.deltaTime * speed);
        }
        else if (Running)
        {
            size = Mathf.Lerp(size, run, Time.deltaTime * speed);
        }
        else if (Jumping)
        {
            size = Mathf.Lerp(size, jump, Time.deltaTime * speed);
        }
        else //idle
        {
            size = Mathf.Lerp(size, idle, Time.deltaTime * speed);
        }

        crosshair.sizeDelta = new Vector2 (size, size);
    }

    public bool Aiming
    {
        get
        {
            if (Input.GetMouseButton(1))
            {
                if(!Walking && !Running && !Jumping)
                {
                    return true;
                }
                else
                {
                    return false;
                }    
            }
            else
            {
                return false;
            }
        }
    }

    bool Walking
    {
        get
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") !=0)
            {
                if (Input.GetKey(KeyCode.LeftShift) == false && !Jumping)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            { 
                return false; 
            }
        }
    }

    bool Running
    {
        get
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) == true && !Jumping)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    bool Jumping
    {
        get
        {
            if (player.GetComponent<Player>().grounded == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
