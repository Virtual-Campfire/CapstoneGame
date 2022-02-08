using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFallCreatureMove : MonoBehaviour
{

    public GameObject Me;
    public float Speed;

    public float RangeAdjust;
    public float HightAdjust;

    Vector3 Wpos;


    // Start is called before the first frame update
    void Start()
    {
        Me = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.PingPong(Time.time * Speed/10, 1) * RangeAdjust/10 + HightAdjust;
        Me.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
