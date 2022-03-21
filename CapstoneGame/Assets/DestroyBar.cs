using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBar : MonoBehaviour
{
    public bool hasStarted;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(GameObject.FindGameObjectWithTag("Bar"));
        Debug.Log("Destroyed the Bar");
    }

    public void swich()
    {
        hasStarted = true;
    }
}
