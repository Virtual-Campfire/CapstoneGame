using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// This script triggers a tutorial sequence script when players enter the game object's trigger
// Can activate tutorials instantly, or after a time is spent in the trigger area (ex. for context-sensitive tutorials)
public class TriggerTutorial : MonoBehaviour
{
    [SerializeField]
    TutorialSequence tutorial;

    GameObject player;

    // Time player has to be in the trigger area before the tutorial appears
    [SerializeField]
    float waitTime = 0, enterTime;
    
    // Whether has stayed in the trigger area before
    bool entered = false, cancelled = false;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // Activate tutorial sequence if player has not exited the trigger area by the waiting time
        if (Time.time >= enterTime + waitTime && entered && !cancelled)
        {
            tutorial.RestartTutorial();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger
        if (other.gameObject == player)
        {
            // Instantly start the tutorial if the waiting time is 0
            if (waitTime == 0)
            {
                tutorial.RestartTutorial();

                return;
            }

            // If this is the player's first entry into the trigger area
            if (!entered)
            {
                entered = true;
                // Save time first entered into trigger zone
                enterTime = Time.time;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the player exits the trigger
        if (other.gameObject == player)
        {
            // Set tutorial to not show up after the waiting time (in other words, the player doesn't need the hint(s) anymore)
            cancelled = true;
        }
    }
}
