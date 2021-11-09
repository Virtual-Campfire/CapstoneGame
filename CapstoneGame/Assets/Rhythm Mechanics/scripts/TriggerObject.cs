using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    // Start is called before the first frame update

    public bool canBepressed1;
    public bool canBepressed2;

    public KeyCode keyToPress;
    public ParticleSystem normalParticles;

    //public float Timer = 0.3f;

    public GameObject Player;

    public BeatController bc;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            if (canBepressed1)
            {
                bc.PlayerSpecialBehavior1();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (canBepressed2)
            {
                bc.PlayerSpecialBehavior2();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Activator1")
        {
            canBepressed1 = true;
        }

        if (other.tag == "Activator2")
        {
            canBepressed2 = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Activator1")
        {
            canBepressed1 = false;
        }

        if (other.tag == "Activator2")
        {
            canBepressed2 = false;
        }
    }
}
