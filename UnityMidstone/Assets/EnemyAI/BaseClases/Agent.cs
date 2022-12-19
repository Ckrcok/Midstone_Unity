using UnityEngine;
using System.Collections;

// Main component to make use of behaviors in order to
// create intelligent movement
public class Agent : MonoBehaviour
{
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;

    // Handles the movements
    protected Steering steering;

    void Start()
    {
        velocity = Vector3.zero;

        // Instantiate the Object for movement
        steering = new Steering();
    }

    // Handles movement according to the current value
    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;

        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;

        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);

    }

    // Takes care of updating steering for the next frame according to the
    // current frame's calculations
    public virtual void LateUpdate()
    {
        // Update the movement using steering for linear and angular movement
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        // Use steering to update the rotation
        if (steering.angular == 0.0f)
            rotation = 0.0f;

        // Use steering to update the velocity
        if (steering.linear.sqrMagnitude == 0.0f)
            velocity = Vector3.zero;

        // Reset the steering for the next update
        steering = new Steering();
    }

    public void SetSteering(Steering steering)
    {
        // Assign the steering reference
        this.steering = steering;
    }
}