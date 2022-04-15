using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Simple script to interpolate a position to another target position; useful for certain camera movement
public class LerpToTarget : MonoBehaviour
{
    [SerializeField]
    Vector3 PlayerPosOffset, PlayerRotOffset;

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
        Quaternion targetRot = target.localRotation;
        Vector3 targetPos = target.position;

        // Add a special offset if with the player
        if (target.gameObject.name == "Ground Ray Base")
        {
            targetRot = Quaternion.Euler(target.localRotation.eulerAngles + PlayerRotOffset);
            targetPos = target.position + PlayerPosOffset;
        }

        if (mode == CameraMoveType.camHard)
        {
            // Smooth follow, with immediate once target is reached
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // Smooth rotation
            transform.localRotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
        }
        else if (mode == CameraMoveType.camSoft)
        {
            // Smooth follow, with slow approach to target (reaches target increasingly slower)
            transform.position += (targetPos - transform.position) * speed * Time.deltaTime;
            // Smooth rotation
            transform.localRotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
        }
        else
        {
            // Rigid follow, no smoothing on the camera's part
            transform.position = targetPos;
            // Rigid rotation
            transform.localRotation = targetRot;
        }
    }
}
