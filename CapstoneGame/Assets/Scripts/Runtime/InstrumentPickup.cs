using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

// Adam B.
// Controls a pickup that adds an instrument/ability to the player's inventory when they run into its trigger
public class InstrumentPickup : MonoBehaviour
{
    [SerializeField]
    int instrumentID;

    [SerializeField]
    GameObject rhythmMechanics;

    public GameObject AudioManager;
    public GameObject BeatController;
    public GameObject BeatScroller;
    public GameObject DestroyBar;
    public GameObject keySpawner1;
    public GameObject keySpawner2;

    void Awake()
    {

        // Temporary code for revealing rhythm mechanics
        MeshRenderer[] temp = rhythmMechanics.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer item in temp)
        {
            item.enabled = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // If player moves within the pickup's range
        if (other.GetComponent<CharacterController_Player>())
        {
            // Add pickup's item ID to player inventory
            other.GetComponent<CharacterController_Player>().AddToInventory(0);

            Debug.Log("Player has picked up an instrument with ID " + instrumentID);

            // Temporary code for revealing rhythm mechanics
            MeshRenderer[] temp = rhythmMechanics.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer item in temp)
            {
                item.enabled = true;
            }

            // Remove the pickup
            Destroy(gameObject);

            //Swich track
            AudioManager.SendMessage("swich");
            BeatController.SendMessage("swich");
            BeatScroller.SendMessage("swich");
            DestroyBar.SendMessage("swich");
            keySpawner1.SendMessage("swich");
            keySpawner2.SendMessage("swich");

            Debug.Log("swich track");
        }
    }
}
