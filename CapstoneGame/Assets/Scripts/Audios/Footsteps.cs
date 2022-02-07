using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string inputAudio;
    public float moveSpeed;

    bool isMoving;

    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= 0.01f || Input.GetAxis("Horizontal") <= 0.01f)
        {
            Debug.Log("Player Moving");
            isMoving = true;
        }
        else if(Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
        {
            Debug.Log("Player not Moving");
            isMoving = false;
        }
    }

    void CallFootsteps()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inputAudio);
    }
}
