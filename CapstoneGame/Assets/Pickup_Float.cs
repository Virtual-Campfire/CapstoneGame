using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Float : MonoBehaviour
{
    //Global Variables
    public float rotSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Check distance to floor/terrain to ensure correct and uniform placement height

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate at smooth, even pace
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
        //Vertical bobbing up and down

    }
}
