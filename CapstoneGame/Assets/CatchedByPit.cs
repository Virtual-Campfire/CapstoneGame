using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatchedByPit : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform PlaceHolder;

    public Transform MyState;

    bool GotMessiage=false;
    // Start is called before the first frame update
    void Start()
    {
        agent = transform.gameObject.GetComponent<NavMeshAgent>();
        MyState = gameObject.transform.Find("States");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetCatched() {

        GotMessiage = true;


       
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);

        if (other.gameObject.layer==20)
        {
           

            MyState.gameObject.SetActive(false);
            


            PlaceHolder = other.transform;
            agent.SetDestination(PlaceHolder.transform.position);
        }
    }

            
        
 }

