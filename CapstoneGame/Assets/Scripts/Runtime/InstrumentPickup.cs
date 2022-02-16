using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

// Adam B.
// Controls a pickup that adds an instrument/ability to the player's inventory when they run into its trigger
public class InstrumentPickup : MonoBehaviour
{
    Vector3 instrumentLoc;

    [SerializeField]
    int instrumentID;

    [SerializeField]
    GameObject instrumentModel;

    [SerializeField]
    GameObject rhythmMechanics;

    public GameObject AudioManager;
    public GameObject BeatController;
    public GameObject BeatScroller;

    void Awake()
    {
        instrumentLoc = instrumentModel.transform.position;

        // Temporary code for revealing rhythm mechanics
        MeshRenderer[] temp = rhythmMechanics.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer item in temp)
        {
            item.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the pickup's model over time
        instrumentModel.transform.rotation = Quaternion.Euler(0, Time.time * 180, 0);

        // Bob the pickup's model up and down over time
        instrumentModel.transform.position = new Vector3(instrumentLoc.x, instrumentLoc.y + 0.25f * Mathf.Sin(Time.time * 3), instrumentLoc.z);
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

            Debug.Log("swich track");
        }
    }
}
