using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    // Start is called before the first frame update

    public bool canBepressed1;
    public bool canBepressed2;

    public KeyCode keyToPress;
    public ParticleSystem normalParticles1;
    public ParticleSystem normalParticles2;

    //public float Timer = 0.3f;

    public GameObject Player;

    public BeatController bc;

    void Awake()
    {
        // Get the player by name
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            if (canBepressed1)
            {
                bc = GameObject.Find("ButtonsFire1").GetComponent<BeatController>();
                //bc.PlayerSpecialBehavior1();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (canBepressed2)
            {
                bc = GameObject.Find("ButtonsFire2").GetComponent<BeatController>();
                //bc.PlayerSpecialBehavior2();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Activator1")
        {
            canBepressed1 = true;
            Instantiate(normalParticles1, Player.transform.position, Quaternion.identity);
        }

        if (other.tag == "Activator2")
        {
            canBepressed2 = true;
            Instantiate(normalParticles2, Player.transform.position, Quaternion.identity);
        }

        if (other.gameObject.tag == "Destroy")
        {
            Debug.Log("destroyed bar");
            Destroy(gameObject);
        }
    }

    /*public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Activator1")
        {
            canBepressed1 = false;
        }

        if (other.tag == "Activator2")
        {
            canBepressed2 = false;
        }
    }*/
}
