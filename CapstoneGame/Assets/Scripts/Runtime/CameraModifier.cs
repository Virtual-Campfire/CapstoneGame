using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// This script moves the player's camera by changing its target location and angle
public class CameraModifier : MonoBehaviour
{
    GameObject player, cam, oldCamNode;

    [SerializeField]
    GameObject newCamNode;

    void Awake()
    {
        player = GameObject.Find("Player");
        cam = GameObject.Find("Camera Base");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Save the camera's previous target
            oldCamNode = cam.GetComponent<LerpToTarget>().target.gameObject;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            // Set the main camera's target to this script's camera node
            cam.GetComponent<LerpToTarget>().target = newCamNode.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            // Set the main camera's target to the player
            cam.GetComponent<LerpToTarget>().target = GameObject.Find("Ground Ray Base").transform;
        }
    }
}
