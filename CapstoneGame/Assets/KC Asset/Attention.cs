using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attention : MonoBehaviour
{


    public float attentionValue = 0;


    [SerializeField]
    float increaseSpeed;
    [SerializeField]
    float decreaseSpeed;

    GameObject player;
    public bool seePlayer;





    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (seePlayer == true)
        {

            //fix the equation problem: it may go - in caculation
            if (dist > GetComponent<FieldOfView>().viewRadius) {
                dist = GetComponent<FieldOfView>().viewRadius;
            }


            attentionValue += (GetComponent<FieldOfView>().viewRadius - dist) / GetComponent<FieldOfView>().viewRadius * increaseSpeed;
            if (attentionValue >= 100)
            {
                attentionValue = 100;
            }
        }

        if (seePlayer == false)
        {
            attentionValue -= Time.deltaTime * decreaseSpeed;
            if (attentionValue <= 0) { attentionValue = 0; }
        }

        seePlayer = false;




        //Debug.Log(attentionValue);
    }
}



