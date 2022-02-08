using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TrackSwich : MonoBehaviour
{
    // private FMOD.Studio.EventInstance instance;

    //  public float trackNumber;

    //  [FMODUnity.EventRef]
    //  public string fmodEvent;
    // public static StudioEventEmitter level1Music;

    [FMODUnity.EventRef]
    public string level1Music;
    [FMODUnity.EventRef]
    public string level1Combat;

    public bool hasStarted;

    void Start()
    {
        // instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        // instance.start();
        //level1Music = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        // instance.setParameterByName("TrackSwich", trackNumber);
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                FMODUnity.RuntimeManager.PlayOneShot(level1Music);
                //   EnvironmentSound.Play();
            }
        }
    }

    public void swich()
    {
        Debug.Log("swich track GET");
       // FMODUnity.RuntimeManager.PlayOneShot(level1Combat);
        // instance.setParameterByName("TrackSwich", trackNumber);
    }
}
