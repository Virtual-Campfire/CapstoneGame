using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject Player;


    public bool ImXylophone;
    public bool ImBell;
    public bool ImHarp;
   

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {

            if (ImXylophone) { Player.SendMessage("FindXylophone"); }
            if (ImBell) { Player.SendMessage("FindBell"); }
            if (ImHarp) { Player.SendMessage("FindHarp"); }

            Destroy(gameObject);
            
        }
    }
}
