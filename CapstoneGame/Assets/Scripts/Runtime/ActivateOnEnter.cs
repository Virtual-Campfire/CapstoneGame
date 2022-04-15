using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Activates a gameobject when trigger is entered
public class ActivateOnEnter : MonoBehaviour
{
    [SerializeField]
    GameObject[] targets;

    void Awake()
    {
        foreach (GameObject item in targets)
        {
            // Set object to be deactivated by default if referenced in this script
            item.SetActive(false);
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
                    item.SetActive(true);
                }
            }
        }
    }
}
