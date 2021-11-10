using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Kills anything with health that touches the trigger
public class KillOnContact : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        DamageKnockback temp = other.GetComponent<DamageKnockback>();

        if (temp != null)
        {
            temp.currentHealth = 0;
        }
    }
}
