using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class SoundStone : MonoBehaviour
{
    public ParticleSystem particles;
    StudioEventEmitter soundEmitter;

    void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        soundEmitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called to activate the particle system and other visual changes for when the stone is hinting what order it is activated in
    public void Chirp()
    {
        soundEmitter.Play();
        particles.Play();
    }

    // Called to set the fade colour when the sequence is correct or incorrect
    public void SetLook(bool result)
    {
        if (result)
        {

        }
        else
        {

        }
    }


}
