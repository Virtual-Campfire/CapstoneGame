using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleSpawn : MonoBehaviour
{
    public Transform shieldPlace;
    public GameObject spark;
    public Transform touchPoint;



    private Rigidbody rb;
    private Collider shieldCollider;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shieldCollider = shieldPlace.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 closestPosition = shieldCollider.ClosestPointOnBounds(transform.position);
       // Debug.Log(closestPosition);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 15)
        {
            Debug.Log("Hit Shield");
            Vector3 closestPosition = shieldCollider.ClosestPointOnBounds(transform.position);
              GameObject clone =  Instantiate(spark, touchPoint.position, Quaternion.identity);
              Destroy(clone, 1f);

        }


       
    }


    void Spark() { 
    //put a spark there
    
    }


}
