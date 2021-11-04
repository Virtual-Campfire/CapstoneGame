using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    // Start is called before the first frame update

    public bool canBepressed;

    public KeyCode keyToPress;
    public ParticleSystem normalParticles;

    //public float Timer = 0.3f;

    public GameObject Player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(keyToPress)){
           // Timer -= Time.deltaTime;

            if (canBepressed)
            {
                Instantiate(normalParticles, Player.transform.position, Quaternion.identity);
                //  gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Activator")
        {
            canBepressed = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Activator")
        {
            canBepressed = false;
        }
    }
}
