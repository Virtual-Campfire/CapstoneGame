using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keySpawner : MonoBehaviour
{
    private float passedTime; // default 0
    public float targetTime;  // set time interval

    public bool hasStarted;

    public GameObject theBars;
    public GameObject spawnerPosition;
    void Update()
    {

        if (hasStarted == true)
        {
            Repete();
        }
    }

    //Repete();   <-This is the call in the function for update.

    void Repete()
    {
        if (passedTime > targetTime)
        {
            //  put function here
            Instantiate(theBars, spawnerPosition.transform.position, spawnerPosition.transform.rotation);

            Debug.Log(Time.time);

            //
            passedTime = 0; //enter next loop
        }
        passedTime += Time.deltaTime;
    }

    public void swich()
    {
        hasStarted = true;
    }
}
