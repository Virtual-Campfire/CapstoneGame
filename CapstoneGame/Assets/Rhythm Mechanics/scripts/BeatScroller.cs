using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class BeatScroller : MonoBehaviour
{
    // Start is called before the first frame update

    public float beatTempo;


    public bool hasStarted;
    
    public static StudioEventEmitter level1Music;

    void Start()
    {
        beatTempo = beatTempo / 60f;
        level1Music = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {

        if (hasStarted == true)
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
            if (transform.position.y <= -20)
            {
                transform.position = new Vector3(12.21481f - 30, -0.7318687f, -43.57994f + 40);
            }
        }
    }

    public void swich()
    {
        hasStarted = true;
    }
}
