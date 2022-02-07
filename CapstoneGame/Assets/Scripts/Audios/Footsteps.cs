using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string inputAudio;
    bool isMoving;
    public float moveSpeed;


    void Update()
    {
        if (Input.GetAxis("Vertical") >= 0.1f || Input.GetAxis("Horizontal") >= 0.1f || Input.GetAxis("Vertical") <= -0.1f || Input.GetAxis("Horizontal") <= -0.1f)
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
        if (isMoving == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(inputAudio);
        }
    }

    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, moveSpeed);
    }

    private void OnDisable()
    {
        isMoving = false;
    }
}
