using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Activates the win sequence for ending the game (this should be on the instrument pickup that wins the game)
public class WinOnEnter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Destroy the pickup
        Destroy(gameObject);

        // Start win sequence
        FindObjectOfType<CharacterController_Player>().StartCoroutine("WinSequence");
    }
}
