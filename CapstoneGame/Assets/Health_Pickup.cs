using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pickup : MonoBehaviour
{
    //Global Variables
    public string parentName;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        parentName = transform.root.name;
    }

    //Triggered Script Upon Entering Collider for Health Pickup
    //Check type of parent health object
    private void OnTriggerEnter(Collider other)
    {
        //Check identity of parent health object
        if (parentName == "Health_Small")
        {
            player.SendMessage("ChangeHPValue", -1);
            Destroy(this);
        }
        else if (parentName == "Health_Large")
        {
            player.SendMessage("ChangeHPValue", -5);
            Destroy(this);
        }
    }
    //Send message of appropriate health collection to health manager script
    //
    //Destroy Parent object once sent

    // Update is called once per frame
    void Update()
    {
        
    }
}
