using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarControl2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float beatTempo;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, beatTempo * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}

