using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHolder : MonoBehaviour
{

    public bool BellFind = false;
    public bool HarpFind = false;
    public bool XylophoneFind = false;

    public GameObject AudioManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }


    void FindXylophone()
    { XylophoneFind = true;
        AudioManager.SendMessage("FindXylophone");
    }
    void FindBell()
    { BellFind = true;
        AudioManager.SendMessage("FindBell");
    }
    void FindHarp()
    { HarpFind = true;
        AudioManager.SendMessage("FindHarp");
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Check1") { XylophoneFind = false; }
        if (other.name == "Check2") { BellFind = false; }
        if (other.name == "Check3") { HarpFind = false; }
    }

}
