using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Oscillates an object, adding an offset
public class OscillateOffset : MonoBehaviour
{
    Vector3 initialPos;

    [SerializeField]
    Vector3 offsetPos;
    [SerializeField]
    float cyclesPerSecond, amplitude;

    void Awake()
    {
        initialPos = transform.position;
    }
    
    // Movement is called on physics update frame, just in case this object has a collider
    void FixedUpdate()
    {
        // Add position offset, then the oscillating position modifier (in relative to the object's local direction for up) to the initial position
        transform.position = initialPos + offsetPos + transform.up * Mathf.Cos(Time.fixedTime * cyclesPerSecond * 2 * Mathf.PI) * amplitude;
    }
}
