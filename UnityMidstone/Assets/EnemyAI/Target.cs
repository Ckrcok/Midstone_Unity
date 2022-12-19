using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 80f;
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name);

        if (health <= 0f)
        {
            Die();
        }
    }
   
    void Die()
    {
        if(gameObject != null)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
