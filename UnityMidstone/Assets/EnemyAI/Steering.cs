using UnityEngine;
using System.Collections;

// Custom data type for storing the movement and rotation of the agent
public class Steering
{

    // Keep values for rotation and forward movement
    public float angular;
    public Vector3 linear;

    public Steering()
    {

        // Initialize the variables
        angular = 0.0f;
        linear = new Vector3();

    }
}
