using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Simple script to interpolate a position to another target position; useful for certain camera movement
public class LerpToTarget : MonoBehaviour
{
    // Different named movement settings for the camera
    public enum CameraMoveType: int
    {
        camSoft,
        camHard,
        camInstant
    };

    // Camera's target transform, speed, and current movement setting
    public Transform target;
    public float speed = 1, rotSpeed = 2;
    public CameraMoveType mode = CameraMoveType.camInstant;

    // Update is called once per frame
    void Update()
    {
        Quaternion targetAngle = target.localRotation;

        if (mode == CameraMoveType.camHard)
        {
            // Smooth follow, with immediate once target is reached
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            // Smooth rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime * rotSpeed);
        }
        else if (mode == CameraMoveType.camSoft)
        {
            // Smooth follow, with slow approach to target (reaches target increasingly slower)
            transform.position += (target.position - transform.position) * speed * Time.deltaTime;
            // Smooth rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime * rotSpeed);
        }
        else
        {
            // Rigid follow, no smoothing on the camera's part
            transform.position = target.position;
            // Rigid rotation
            transform.rotation = targetAngle;
        }
    }
}
