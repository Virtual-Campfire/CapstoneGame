using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItem : MonoBehaviour
{
    public GameObject TargetToShow;

    
    GameObject player;

    public bool Key1;
    public bool Key2;
    public bool Key3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player.GetComponent<PlayerItemHolder>().XylophoneFind == true && Key1)
            {
                TargetToShow.SetActive(true);
            }
            

            if (player.GetComponent<PlayerItemHolder>().BellFind == true && Key2)
            {
                TargetToShow.SetActive(true);
            }
            if (player.GetComponent<PlayerItemHolder>().HarpFind == true && Key3)
            {
                TargetToShow.SetActive(true);
            }



        }
    }

}
