using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    GameController gameController;      

    // Interaction Setup
    public static bool itemCollected = false;
    public string newTag;
    public Text TagText;

    void Start()
    {
        // Find the game object that has the GameController component        
        itemCollected = false;        
    }

    private void Update()
    {
  
    }

    public void MoveGameObject()
    {
        transform.position = new Vector3(0f, 100f, 0f);
    }


    public void ChangeTag()
    {
        gameObject.tag = newTag;
        TagText.text = newTag;
    }
}
