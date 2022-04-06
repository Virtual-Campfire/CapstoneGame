using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

// Adam B.
// Controls the sound stones in the puzzle, including their retrieval if they are off their pedestal
public class SoundStone : MonoBehaviour
{
    public ParticleSystem particlesNeutral, particlesRight, particlesWrong;
    StudioEventEmitter soundEmitter;

    [SerializeField]
    GameObject player, statue;
    Collider trigger;

    Vector3 initialPos;

    [SerializeField]
    float activationDist = 5;
    
    // Variable for whether or not the stone base has its statue and is ready to work
    public bool isWhole = false;

    void Awake()
    {
        soundEmitter = GetComponent<StudioEventEmitter>();

        player = GameObject.Find("Player");

        // If the statue is on its pedestal, the sound stone is complete and can be used in the puzzle
        if (statue.transform.position == transform.position)
        {
            isWhole = true;
        }

        initialPos = statue.transform.position;
    }

    void Update()
    {
        // When player is near the statue if it's separated from its base
        if (Vector3.Distance(player.transform.position, statue.transform.position) < activationDist && !isWhole)
        {
            // Set position of stone behind player
            statue.transform.position = player.transform.position;
            statue.transform.position += -player.transform.forward * 2;
            // Reset rotation in case the statue was placed on its side
            statue.transform.rotation = Quaternion.identity;

            // Put statue on base if they have been moved close together after being carried
            if (Vector3.Distance(player.transform.position, transform.position) < activationDist)
            {
                // Set position of statue over stone base
                statue.transform.position = transform.position;
                statue.transform.position += Vector3.up * 2;
                // Sound stone is now whole
                isWhole = true;
                // Tell main puzzle script to check if all statues are in place
                GetComponentInParent<SoundSequencePuzzle>().CheckPuzzle();
            }
        }
        else if (!isWhole)
        {
            // If player gets far away and loses hold of the statue, it returns to its initial position so it doesn't become inaccessible (ex. on player respawn after falling into a pit)
            statue.transform.position = initialPos;
        }

        if (isWhole)
        {
            // Causes smooth return to position after pulsing down
            statue.transform.position = Vector3.MoveTowards(statue.transform.position, transform.position + Vector3.up * 2, Time.deltaTime);
        }
    }

    // Called to activate the particle system and other visual changes for when the stone is hinting what order it is activated in
    public void Chirp()
    {
        ChimeAndMove();
        particlesNeutral.Play();
    }

    public void ChirpRight()
    {
        ChimeAndMove();
        particlesRight.Play();
    }

    public void ChirpWrong()
    {
        ChimeAndMove();
        particlesWrong.Play();
    }

    void ChimeAndMove()
    {
        // Play sound
        soundEmitter.Play();

        // Moves statue down when it makes a chirp (in case player cannot hear the sound or see the particles)
        statue.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }
}
