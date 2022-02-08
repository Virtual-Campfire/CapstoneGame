using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    // Start is called before the first frame update

    public KeyCode keyToPress;
    public ParticleSystem specialParticles;

    public GameObject Player;
    // Container for player controller
    public CharacterController_Player controller;


     private FMOD.Studio.EventInstance instance;

     [FMODUnity.EventRef]
     public string fmodEvent;


     float timer = 0;

     public float hitValue;
     

    void Start()
    {
        // Get player controller script
        controller = Player.GetComponent<CharacterController_Player>();

        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        hitValue -= Time.deltaTime * 10;
        instance.setParameterByName("hit", hitValue);
    }

    public void PlayerSpecialBehavior1()
    {
        // normal particles
        Instantiate(specialParticles, Player.transform.position, Quaternion.identity);
        Debug.Log("PlayerSpecialBehavior 1 Triggerd !!!!!! ");


        // Modify the attack
        controller.specialMelee = true;


        // Trigger audio
        hitValue = 10;
        instance.setParameterByName("hit", hitValue);
        
    //    if (hitValue == 0)
    //    {
    //        instance.setParameterByName("hit", 0);
    //    }


    }

    public void PlayerSpecialBehavior2()
    {
        Debug.Log("PlayerSpecialBehavior 2 Triggerd !!!!!! ");
    }

    public void swich()
    {
        instance.start();
    }

}
