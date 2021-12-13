using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdealState : FSMState
{

    public UnityEngine.AI.NavMeshAgent agent;

    public Transform SaveLocation;

    public float rotationResetSpeed;
    GameObject Player;


    void Awake()
    {
        stateID = StateID.Ideal;
        AddTransition(Transition.IntoDoubt, StateID.Doubt);
        AddTransition(Transition.IntoPatrol, StateID.Patrol);
        AddTransition(Transition.IntoDead, StateID.Dead);
        AddTransition(Transition.IntoLure, StateID.Lure);
        
        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();


        Player = GameObject.Find("Player");
    }


    private void Start()
    {
        SaveLocation = GetComponentInParent<EnemyState>().PositionHolder.transform;
        
    }


    private void Update()
    {
        if (GetComponentInParent<Attention>().attentionValue == 0) {

            GetComponentInParent<EnemyState>().ReturnFromChase = false;
        }



        //Debug.Log(Vector3.Distance(transform.position, SaveLocation.position));
    }

    public override void DoBeforeEntering()
    {
       Debug.Log("I am ready in to the Ideal now");

        GetComponent<DeadState>().Ma.SetFloat("_Control", 0);
        GetComponent<DeadState>().Ma.SetFloat("_control", 0);

    }

    public override void Act()
    {
        Debug.Log("NPC In ideal");


        //if not at origanal position, move back to where it start
        ResetPosition(); 


    }


    // Adam: Lure functionality will be called from CharacterController_Player.cs
    public override void Reason()
    {
        
         manager.Fsm.PerformTransition(Transition.IntoLure);
     


        if (GetComponentInParent<EnemyState>().HP <= 0) {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }


            if (GetComponentInParent<EnemyState>().IsPatrol == true) {
            manager.Fsm.PerformTransition(Transition.IntoPatrol);

        }

        if (GetComponentInParent<Attention>().attentionValue>10) { manager.Fsm.PerformTransition(Transition.IntoDoubt); }
       
    }

    public override void DoBeforeLeaving()
    {
        Debug.Log("Leaving Ideal");
    }






    //below is behavior code
    void ResetPosition() {
        if (Vector3.Distance(transform.position, SaveLocation.position) >= 1) {
            agent.SetDestination(SaveLocation.position);
        }else if (Vector3.Distance(transform.position, SaveLocation.position) < 1){
            ResetRotaion();
        }
    
    }

    void ResetRotaion() {
        if (transform.parent.rotation != SaveLocation.rotation) {

            transform.parent.rotation = Quaternion.Slerp(transform.rotation, SaveLocation.rotation, Time.time * rotationResetSpeed);


        }
    
    }


}
