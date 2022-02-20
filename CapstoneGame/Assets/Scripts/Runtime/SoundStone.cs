using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

// Adam B.
// Controls the sound stones in the puzzle, including their retrieval if they are off their pedestal
public class SoundStone : MonoBehaviour
{
    public ParticleSystem particles;
    StudioEventEmitter soundEmitter;

    [SerializeField]
    GameObject player, statue;
    Collider trigger;

    [SerializeField]
    float activationDist = 5;
    
    // Variable for whether or not the stone base has its statue and is ready to work
    public bool isWhole = false;

    void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        soundEmitter = GetComponent<StudioEventEmitter>();

        player = GameObject.Find("Player");

        // If the statue is on its pedestal, the sound stone is complete and can be used in the puzzle
        if (statue.transform.position == transform.position)
        {
            isWhole = true;
        }
    }

    void Update()
    {
        // When player is near the statue if it's separated from its base
        if (Vector3.Distance(player.transform.position, statue.transform.position) < activationDist && !isWhole)
        {
            // Set position of stone behind player
            statue.transform.position = player.transform.position;
            statue.transform.position += -player.transform.forward * 2;

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
    }

    // Called to activate the particle system and other visual changes for when the stone is hinting what order it is activated in
    public void Chirp()
    {
        soundEmitter.Play();
        particles.Play();
    }
}
