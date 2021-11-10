using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    // Start is called before the first frame update

    public float beatTempo;
    public AudioSource level1Music;

    public bool hasStarted;

    void Start()
    {
        beatTempo = beatTempo / 60f;
        level1Music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/level1BGM");
            }
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }

        if (transform.position.y <= -112.7)
        {
            transform.position = new Vector3(12.21481f -30 , -0.7318687f, -43.57994f +40);
        }
        //transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
    }
}
