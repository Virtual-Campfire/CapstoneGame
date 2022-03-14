using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerPass : MonoBehaviour
{
   public  GameObject MyTarget;





    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            MyTarget.SendMessage("TurnOn");
        }
    }
}
