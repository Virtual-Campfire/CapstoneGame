using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Adam B.
// Script that controls text-based tutorial sequences (with minor interactivity and responses to player actions and key presses to aid in teaching)
public class TutorialSequence : MonoBehaviour
{
    public int index = 0;

    [Serializable]
    public struct Tutorials
    {
        public Text[] text;
        public Image[] images;
        public bool playerLock;
    }

    [SerializeField]
    public Tutorials[] tutorialPieces;

    [SerializeField]
    CharacterController_Player player;

    [SerializeField]
    float timeSinceLast, timeBetween = 1;
    
    bool tutorialFinished = true;

    void Awake()
    {
        player = FindObjectOfType<CharacterController_Player>();
    }

    void Update()
    {
        // If tutorial sequence has not ended
        if (index <= tutorialPieces.Length && !tutorialFinished)
        {
            // If any key is pressed
            if (Input.anyKeyDown)
            {
                CycleTutorial();
            }
        }
        else
        {
            // Tutorial has finished; this value can be reset to start the tutorial again
            tutorialFinished = true;
        }
    }

    void CycleTutorial()
    {
        // Do not cycle to next tutorial step unless time has passed so that players don't accidently cycle through all of them at once
        if (Time.time > timeSinceLast + timeBetween)
        {
            // Save time that this tutorial stage was cycled in
            timeSinceLast = Time.time;

            // Hide last stage in tutorial
            if (index > 0)
            {
                foreach (Text item in tutorialPieces[index - 1].text)
                {
                    item.enabled = false;
                }
                foreach (Image item in tutorialPieces[index - 1].images)
                {
                    item.enabled = false;
                }
            }

            // Show next stage in tutorial
            if (index < tutorialPieces.Length)
            {
                foreach (Text item in tutorialPieces[index].text)
                {
                    item.enabled = true;
                }
                foreach (Image item in tutorialPieces[index].images)
                {
                    item.enabled = true;
                }

                // Sets whether the player is able to move or not during this tutorial step
                if (tutorialPieces[index].playerLock)
                {
                    player.moveLock = true;
                }
                else
                {
                    player.moveLock = false;
                }
            }
            else
            {
                // Unlock player movement if still locked
                player.moveLock = false;
            }

            // Next time function is called, index will be for the next stage of the tutorial
            index++;
        }

        // Special tutorial sequences and conditionals on certain index numbers?
    }

    // This can be called by other functions to show the tutorial sequence any number of times
    public void RestartTutorial()
    {
        index = 0;
        tutorialFinished = false;
        CycleTutorial();
    }
}
