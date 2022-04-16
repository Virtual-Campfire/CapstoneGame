using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Activates forklifts when trigger is entered
public class ActivateOnEnter : MonoBehaviour
{
    [SerializeField]
    GameObject[] targets;

    void Awake()
    {
        foreach (GameObject item in targets)
        {
            if (item.GetComponent<ForkliftBehaviour>())
            {
                // Set object to be deactivated by default if referenced in this script, unless it is a forklift that is set to wait
                if (!item.GetComponent<ForkliftBehaviour>().waitForTrigger)
                {
                    item.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject item in targets)
            {
                if (item)
                {
                    // Set the forklift to active if it's not already
                    item.SetActive(true);
                    
                    // If the forklift is waiting for an external trigger to start its movement behaviour, start it
                    if (item.GetComponent<ForkliftBehaviour>().waitForTrigger)
                    {
                        item.GetComponent<ForkliftBehaviour>().waitForTrigger = false;
                        item.GetComponent<ForkliftBehaviour>().GoToNextNode();
                    }
                }
            }
        }
    }
}
