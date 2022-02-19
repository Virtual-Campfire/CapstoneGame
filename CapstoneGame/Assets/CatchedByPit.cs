using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatchedByPit : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform PlaceHolder;

    bool GotMessiage=false;
    // Start is called before the first frame update
    void Start()
    {
        agent = transform.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetCatched() {

        GotMessiage = true;


        Debug.Log("yesyesyesyes");
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);

        if (other.tag == "PitFall")
        {
           gameObject.GetComponent<EnemyState>().enabled=false;
            PlaceHolder = other.transform;
            agent.SetDestination(PlaceHolder.transform.position);
        }
    }

            
        
 }

