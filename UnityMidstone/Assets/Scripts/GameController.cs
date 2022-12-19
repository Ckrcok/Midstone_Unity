using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int collectible = 0;
    public bool isAllCollected = false;
    // Start is called before the first frame update
    void Start()
    {
        if (collectible >= 3)
        {
            isAllCollected = true;
        }
        else isAllCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Collectible: " + collectible);
    }
}
