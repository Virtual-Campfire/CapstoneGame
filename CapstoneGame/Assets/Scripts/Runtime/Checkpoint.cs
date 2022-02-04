using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Controls a checkpoint that players can get activate by entering a trigger zone so that they may respawn at the checkpoint later
public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    Transform checkpointLocation;

    void OnTriggerEnter(Collider other)
    {
        // If player enters checkpoint area save checkpoint location to player
        if (other.GetComponent<CharacterController_Player>())
        {
            other.GetComponent<CharacterController_Player>().lastCheckpoint = checkpointLocation.position;

            Debug.Log("Player has activated a checkpoint at: " + checkpointLocation.position);

            // There will likely be things to do besides saving the player's checkpoint location in later versions (this is one reason why this is a unique script)
        }
    }
}
