using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Pickup : MonoBehaviour
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

    //Triggered Script Upon Entering Collider for Music Pickup

    //Send message of appropriate music power collection to music power manager script
    //
    //Destroy Parent object once sent

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Resource before collection: " + player.GetComponent<CharacterController_Player>().currentResource);
        //player.GetComponent<CharacterController_Player>().AddResource(20.0f);
        player.GetComponent<CharacterController_Player>().currentResource += 20.0f;
        Debug.Log("Resource after collection: " + player.GetComponent<CharacterController_Player>().currentResource);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
