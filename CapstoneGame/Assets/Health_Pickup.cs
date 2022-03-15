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

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    //Triggered Script Upon Entering Collider for Health Pickup
    //Check type of parent health object
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger has been entered");
        //Check identity of parent health object
        if (parentName == "Health_Small")
            smallHeal();
        else if (parentName == "Health_Large")
            bigHeal();
        else
            Debug.Log("Unrecognized Pickup");
    }
    //Send message of appropriate health collection to health manager script
    //
    //Destroy Parent object once sent

    // Update is called once per frame
    void Update()
    {
        
    }

    void smallHeal()
    {
        player.GetComponent<DamageKnockback>().ApplyDamage(-1);
        Destroy(gameObject);
    }

    void bigHeal()
    {
        player.GetComponent<DamageKnockback>().ApplyDamage(-5);
        Destroy(gameObject);
    }

}
