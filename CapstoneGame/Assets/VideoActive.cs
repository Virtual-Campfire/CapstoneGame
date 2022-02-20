using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoActive : MonoBehaviour
{
    public GameObject Target;
    public float MyTimer;
    bool Once = true;
    

    private void Update()
    {

        //    if (Target == null) {
        //    GetComponent<RawImage>().enabled = true;
        //    MyTimer = MyTimer - Time.deltaTime;
            
        //}


        //    if (MyTimer <= 0 && Once == true)
        //    {
        //        GetComponent<RawImage>().enabled = false;
        //    }

    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.tag == "Player") { GetComponent<RawImage>().enabled = true; }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player") { GetComponent<RawImage>().enabled = false; }

    }
}
