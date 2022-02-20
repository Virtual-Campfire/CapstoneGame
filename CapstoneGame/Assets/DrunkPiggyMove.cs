using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkPiggyMove : MonoBehaviour
{
    public GameObject TargetPiggy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && TargetPiggy != null) {

            TargetPiggy.GetComponent<EnemyState>().IsPatrol = true;
        
        }
    }
}
