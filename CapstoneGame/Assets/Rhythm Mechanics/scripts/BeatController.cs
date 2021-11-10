using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    // Start is called before the first frame update

    public KeyCode keyToPress;
    public ParticleSystem normalParticles;

    public GameObject Player;
    // Container for player controller
    public CharacterController_Player controller;

    //Animator _animator;

    void Start()
    {
        // Get player controller script
        controller = Player.GetComponent<CharacterController_Player>();
    }

    private void Awake()
    {
        // _animator = GetComponent<Animator>();
        //_animator = Player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           // PlayerSpecialBehavior1();
        }

        if (Input.GetButtonUp("Fire1"))
        {
           // _animator.ResetTrigger("Attack1");
        }

        if (Input.GetButtonDown("Fire2"))
        {
           // PlayerSpecialBehavior2();
        }

        if (Input.GetButtonUp("Fire2"))
        {
          //  _animator.ResetTrigger("Attack2");
        }
    }

    public void PlayerSpecialBehavior1()
    {
        // normal particles
        // Instantiate(normalParticles, transform.position, Quaternion.identity);
        Debug.Log("PlayerSpecialBehavior 1 Triggerd !!!!!! ");


        // Modify the attack
        controller.meleeKnockback = 5;
        controller.meleeDamage = 5;
    }

    public void PlayerSpecialBehavior2()
    {
        // normal particles
        // Instantiate(normalParticles, transform.position, Quaternion.identity);
        Debug.Log("PlayerSpecialBehavior 2 Triggerd !!!!!! ");

    }


}
