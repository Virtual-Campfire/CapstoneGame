using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attention : MonoBehaviour
{


    float value = 0;


    [SerializeField]
    float increaseSpeed;
    [SerializeField]
    float decreaseSpeed;


    public GameObject player;





    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= GetComponent<FieldOfView>().viewRadius)
        {
            value += (GetComponent<FieldOfView>().viewRadius - dist) / GetComponent<FieldOfView>().viewRadius * increaseSpeed;
            if (value >= 100)
            {
                value = 100;
            }
        }
        if (dist > GetComponent<FieldOfView>().viewRadius)
        {
            value -= Time.deltaTime * decreaseSpeed;
            if (value < 0) { value = 0; }
        }

        Debug.Log(value);


    }
}
