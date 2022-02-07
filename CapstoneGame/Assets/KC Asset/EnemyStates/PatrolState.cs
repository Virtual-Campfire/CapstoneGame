using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : FSMState
{

    public UnityEngine.AI.NavMeshAgent agent;

    public Transform[] points;
    private int destPoint = 0;

    GameObject Player;


    void Awake()
    {
        stateID = StateID.Patrol;
        AddTransition(Transition.IntoDoubt, StateID.Doubt);
        AddTransition(Transition.IntoDead, StateID.Dead);
        AddTransition(Transition.IntoLure, StateID.Lure);

        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();


        Player = GameObject.Find("Player");
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

        

        if (GetComponentInParent<Attention>().attentionValue > 0) { agent.SetDestination(transform.parent.position); }
        else
        {

            if (!agent.pathPending && agent.remainingDistance < 0.5f) { PatrolLoop(); }


        }

    }



    public override void Reason()
    {
        if (GetComponentInParent<EnemyState>().HP <= 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }

        if (Player.GetComponent<PlayerController>())
        {
            if (Player.GetComponent<PlayerController>().PlayLure == true && GetComponentInParent<EnemyState>().DisToPlayer <= Player.GetComponent<PlayerController>().Range)
            {
                manager.Fsm.PerformTransition(Transition.IntoLure);
            }
        }

        if (Player.GetComponent<CharacterController_Player>())
        {
            // Alternative variable check
            if (Player.GetComponent<CharacterController_Player>().playingLure == true && GetComponentInParent<EnemyState>().DisToPlayer <= GetComponent<LureState>().LureRange)
            {
                manager.Fsm.PerformTransition(Transition.IntoLure);
            }
        }

        if (GetComponentInParent<Attention>().attentionValue > 10) { manager.Fsm.PerformTransition(Transition.IntoDoubt); }
    }


    void PatrolLoop()
    {
        if (points.Length == 0) { Debug.Log("Patrol check 'bool==True'  but no point set!!"); return; }
           
        

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;


    }
}
