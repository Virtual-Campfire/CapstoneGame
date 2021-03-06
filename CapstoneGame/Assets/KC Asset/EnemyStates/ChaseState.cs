using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : FSMState
{
    // Temporary character animator controller
    [SerializeField]
    Animator anim;

    public NavMeshAgent agent;
    public GameObject Player;
    float dist;

    //link attack range with the setting range from here
    public float AttackRange, FireBallRange;

    // Can be set to make AI enemy stay in the same spot while firing or otherwise set
    public bool holdPosition;

    void Awake()
    {
        stateID = StateID.Chasing;
        AddTransition(Transition.IntoIdeal, StateID.Ideal);
        AddTransition(Transition.IntoAttack, StateID.Attack);
        AddTransition(Transition.IntoDead, StateID.Dead);

        if (Player == null) { Player = GameObject.Find("Player"); }


        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();

        AttackRange = GetComponent<attackState>().AttackRange;
        FireBallRange = GetComponent<attackState>().FireBallRange;

        //anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        dist = Vector3.Distance(Player.transform.position, this.transform.parent.position);
        // Debug.Log(dist);

        // Control animator parameters
        //anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    
    public override void DoBeforeEntering()
    {
        //Debug.Log("I am ready in to the chasing now");
    }



    public override void DoBeforeLeaving()
    {
       
    }


    public override void Reason()
    {
        if (GetComponentInParent<EnemyState>().HP <= 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }


        if (GetComponentInParent<Attention>().attentionValue < 50 )
        {
            GetComponentInParent<EnemyState>().ReturnFromChase = true;
            manager.Fsm.PerformTransition(Transition.IntoIdeal);
        }

        if (dist <= AttackRange || (GetComponent<attackState>().EnemyDevilHead && dist <= FireBallRange) ) {
            manager.Fsm.PerformTransition(Transition.IntoAttack);
        }
    }

    public override void Act()
    {
        if (!holdPosition)
        {
            agent.SetDestination(Player.transform.position);
        }
        else
        {
            // If holding position if set to (happens when charging ranged attack)
            agent.SetDestination(transform.position);
        }
    }




}
