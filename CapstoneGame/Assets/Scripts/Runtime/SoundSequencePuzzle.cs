using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

// Adam B.
// This script manages a sequence puzzle that the player interacts with using their music
// Players must activate the pieces in the sequence that they show
public class SoundSequencePuzzle : MonoBehaviour
{
    public DoorTurning[] Gates;


    [SerializeField]
    GameObject[] disappearingParts;
    [SerializeField]
    GameObject[] pieces;
    [SerializeField]
    int patternCompletion = 0;
    [SerializeField]
    bool constructed, complete;
    [SerializeField]
    float activationDistance = 10;

    [SerializeField] [Tooltip("Chirp Frequency is the time in seconds between each puzzle piece making a noice during when hinting the answer; Chirp Pause is the time in seconds between restarting the hint sequence from the beginning.")]
    float chirpFrequency = 1, chirpPause = 3, delayBetweenInputs = 1;
    float timeOfLastChirp, timeSinceInput;

    CharacterController_Player player;

    void Awake()
    {
        // Get the player controller script by its type
        player = FindObjectOfType<CharacterController_Player>();

        // Check if the main puzzle is ready to start (it will not if stone bases lack their statues)
        CheckPuzzle();
    }

    public void CheckPuzzle()
    {
        // Set default value until disproven (if statues are not on their stone bases)
        constructed = true;

        // Check if all sound stone bases have their statues
        foreach (GameObject item in pieces)
        {
            if (!item.GetComponent<SoundStone>().isWhole)
            {
                constructed = false;
            }
        }

        // Start puzzle hinting loop if all stone bases have their statues
        if (constructed)
        {
            timeOfLastChirp = Time.time;
            StartCoroutine("Step");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If puzzle is incomplete and player is nearby AND statues are on the bases of all sound stones
        if (!complete && player.playingLure && Time.time > timeSinceInput + delayBetweenInputs && Vector3.Distance(player.gameObject.transform.position, transform.position) <= activationDistance && constructed)
        {
            // Stop hinting system when
            StopCoroutine("Step");

            GameObject closestPiece = pieces[0];
            float dist = 1000000;

            // Check distance between each piece and the player, until the closest piece is found and stored in memory
            foreach(GameObject item in pieces)
            {
                float temp = Vector3.Distance(item.transform.position, player.gameObject.transform.position);

                if (temp < dist)
                {
                    // Set current piece at the closest piece to player
                    closestPiece = item;
                    dist = temp;
                }
            }

            // Indicate this piece has been activated
            closestPiece.GetComponent<SoundStone>().Chirp();

            // If the activated piece is the same as the next one in the sequence, let the player keep going
            if (closestPiece == pieces[patternCompletion])
            {
                // If sequence is fully complete without a wrong input
                if (patternCompletion == pieces.Length - 1)
                {
                    RightAnswer();
                }

                // Iterate value so this conditional statement checks for the next piece in the sequence next time a piece is activated
                patternCompletion++;
            }
            else
            {
                WrongAnswer();
            }

            // Makes it so duplicate inputs don't happen by setting a cooldown for activating the next element
            timeSinceInput = Time.time;
        }
    }

    void WrongAnswer()
    {
        // Feedback that player's answer is incorrect
        foreach (GameObject item in pieces)
        {
            ParticleSystem temp = item.GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule temp2 = temp.main;
            temp2.startColor = Color.red;
            item.GetComponent<SoundStone>().Chirp();
        }

        // Reactivate hinting system
        StopCoroutine("Step");
        StartCoroutine("Step");

        // Reset progress completing puzzle sequence
        patternCompletion = 0;
    }

    void RightAnswer()
    {
        if (!complete)
        {
            // Feedback that player's answer is correct
            foreach (GameObject item in pieces)
            {
                ParticleSystem temp = item.GetComponentInChildren<ParticleSystem>();
                ParticleSystem.MainModule temp2 = temp.main;
                temp2.startColor = Color.green;
                item.GetComponent<SoundStone>().Chirp();
            }

            // Disable doors related to this puzzle
            foreach (GameObject item in disappearingParts)
            {
                item.SetActive(false);
            }
            foreach (DoorTurning doorTurning in Gates)
            {
                doorTurning.OpenTheDoor = true;
            }
            // Stop hinting system coroutine
            StopCoroutine("Step");

            // So this does not trigger again
            complete = true;
        }
    }

    //// When player enters puzzle area
    //void OnTriggerEnter(Collider other)
    //{
    //    // If object entering is the player
    //    if (other.GetComponent<CharacterController_Player>())
    //    {
    //        // Fade music in later versions
    //    }
    //}

    //// When player exits puzzle area
    //void OnTriggerExit(Collider other)
    //{
    //    // If object exiting is the player
    //    if (other.GetComponent<CharacterController_Player>())
    //    {
    //        // Fade music in later versions
    //    }
    //}

    // Controls the sequence hinting
    IEnumerator Step()
    {
        while (true)
        {
            // Call each stone to make a sound in sequence
            for (int i = 0; i < pieces.Length; i++)
            {
                ParticleSystem.MainModule temp = pieces[i].GetComponentInChildren<SoundStone>().particles.main;
                temp.startColor = Color.white;
                pieces[i].GetComponent<SoundStone>().Chirp();

                yield return new WaitForSeconds(chirpFrequency);
            }

            // Wait a specified time before restarting the sequence
            yield return new WaitForSeconds(chirpPause);
        }
    }
}
