using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LureState : FSMState
{
    GameObject Player;

    float LureRange;

     UnityEngine.AI.NavMeshAgent agent;




    public float DelayBeforeDoing;

    private void Awake()
    {
        stateID = StateID.Lure;

        AddTransition(Transition.IntoDoubt, StateID.Doubt);

        AddTransition(Transition.IntoChasing, StateID.Chasing);

        AddTransition(Transition.IntoDead, StateID.Dead);

        AddTransition(Transition.IntoIdeal, StateID.Ideal);
        AddTransition(Transition.IntoLure, StateID.Lure);



        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        LureRange=Player.GetComponent<PlayerController>().Range;

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
        float dis= Vector3.Distance(transform.position, Player.transform.position);

        if (LureRange > dis && Player.GetComponent<PlayerController>().PlayLure == true)
        {

            agent.SetDestination(Player.transform.position);

        }
        else { agent.SetDestination(transform.position); }

 


    }



    public override void Reason()
    {
        if (GetComponentInParent<EnemyState>().HP <= 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }

        if (GetComponentInParent<Attention>().attentionValue > 10)
        {
            manager.Fsm.PerformTransition(Transition.IntoDoubt);
        }

        if (GetComponentInParent<Attention>().attentionValue >= 50)
        {
            manager.Fsm.PerformTransition(Transition.IntoChasing);
        }

        if (GetComponentInParent<Attention>().attentionValue <= 10 && Player.GetComponent<PlayerController>().PlayLure==false && GetComponentInParent<EnemyState>().Timer<0) {

            if (GetComponentInParent<EnemyState>().LastInput==true) { Invoke("Return", DelayBeforeDoing); }
            

        }

        if (Player.GetComponent<PlayerController>().PlayLure == true && GetComponentInParent<EnemyState>().DisToPlayer <= Player.GetComponent<PlayerController>().Range) {
            manager.Fsm.PerformTransition(Transition.IntoLure);
        }

        // Alternative variable check
        if (Player.GetComponent<CharacterController_Player>().playingLure == true && GetComponentInParent<EnemyState>().DisToPlayer <= Player.GetComponent<PlayerController>().Range)
        {
            manager.Fsm.PerformTransition(Transition.IntoLure);
        }


    }


    void Return() {

        manager.Fsm.PerformTransition(Transition.IntoIdeal);
    }

   

}
