using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : FSMState
{

    public UnityEngine.AI.NavMeshAgent agent;

    public Transform[] points;
    private int destPoint = 0;


    void Awake()
    {
        stateID = StateID.Patrol;
        AddTransition(Transition.IntoDoubt, StateID.Doubt);

        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();



    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public override void Act()
    {

        Debug.Log("In Patrol");

        if (GetComponentInParent<Attention>().attentionValue > 0) { agent.SetDestination(transform.parent.position); }
        else
        {

            if (!agent.pathPending && agent.remainingDistance < 0.5f) { PatrolLoop(); }


        }

    }



    public override void Reason()
    {



        if (GetComponentInParent<Attention>().attentionValue > 10) { manager.Fsm.PerformTransition(Transition.IntoDoubt); }
    }


    void PatrolLoop()
    {
        if (points.Length == 0) { Debug.Log("Patrol check 'bool==True'  but no point set!!"); return; }
           
        

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;


    }
}
