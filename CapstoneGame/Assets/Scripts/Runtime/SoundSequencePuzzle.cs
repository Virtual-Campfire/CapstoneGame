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
    [SerializeField]
    GameObject[] doors;
    [SerializeField]
    GameObject[] pieces;
    [SerializeField]
    int patternCompletion = 0;
    [SerializeField]
    bool complete;

    [SerializeField] [Tooltip("Chirp Frequency is the time in seconds between each puzzle piece making a noice during when hinting the answer; Chirp Pause is the time in seconds between restarting the hint sequence from the beginning.")]
    float chirpFrequency = 1, chirpPause = 3;
    float timeOfLastChirp, timeSinceInput;

    CharacterController_Player player;

    void Awake()
    {
        // Get the player controller script by its type
        player = FindObjectOfType<CharacterController_Player>();
        timeOfLastChirp = Time.time;
        StartCoroutine("Step");
    }

    // Update is called once per frame
    void Update()
    {
        if (!complete && player.playingLure && Time.time > timeSinceInput + 1)
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
            ParticleSystem.MainModule temp = item.GetComponent<SoundStone>().particles.main;
            temp.startColor = Color.red;
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
                ParticleSystem.MainModule temp = item.GetComponent<SoundStone>().particles.main;
                temp.startColor = Color.green;
                item.GetComponent<SoundStone>().Chirp();
            }

            // Disable doors related to this puzzle
            foreach (GameObject item in doors)
            {
                item.SetActive(false);
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
                ParticleSystem.MainModule temp = pieces[i].GetComponent<SoundStone>().particles.main;
                temp.startColor = Color.white;
                pieces[i].GetComponent<SoundStone>().Chirp();

                yield return new WaitForSeconds(chirpFrequency);
            }

            // Wait a specified time before restarting the sequence
            yield return new WaitForSeconds(chirpPause);
        }
    }
}
