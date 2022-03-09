using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Check : MonoBehaviour
{
    //Global Variables
    public string parentName;
    public GameObject A;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Triggered Script Upon Entering Collider for Health Pickup
    //Check type of parent health object
    private void OnTriggerEnter(Collider other)
    {
        //Check identity of parent health object
        if (parentName == "Health_Small")
        {
            A.SendMessage("SelfDestruct");
            //Change Health value
        }
        else if (parentName == "Health_Large")
        {
            A.SendMessage("SelfDestruct");
            //Change Health value
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
