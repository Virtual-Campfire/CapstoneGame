using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// This script triggers a tutorial sequence script when players enter the game object's trigger
public class TriggerTutorial : MonoBehaviour
{
    [SerializeField]
    TutorialSequence tutorial;

    GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger, start the tutorial
        if (other.gameObject == player)
        {
            tutorial.RestartTutorial();
        }
    }
}
