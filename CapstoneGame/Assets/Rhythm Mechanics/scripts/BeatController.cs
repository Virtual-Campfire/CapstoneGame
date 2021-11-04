using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    // Start is called before the first frame update

    public KeyCode keyToPress;
    public ParticleSystem normalParticles;

    public GameObject Player;

    Animator _animator;

    void Start()
    {

    }

    private void Awake()
    {
        // _animator = GetComponent<Animator>();
        _animator = Player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayerNormalBehavior();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
           // _animator.ResetTrigger("Attack1");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerNormalBehavior2();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
          //  _animator.ResetTrigger("Attack2");
        }
    }

    public void PlayerNormalBehavior()
    {
        // normal particles
       // Instantiate(normalParticles, transform.position, Quaternion.identity);
        _animator.SetTrigger("Attack1");

    }

    public void PlayerNormalBehavior2()
    {
        // normal particles
       // Instantiate(normalParticles, transform.position, Quaternion.identity);
        _animator.SetTrigger("Attack2");

    }


}
