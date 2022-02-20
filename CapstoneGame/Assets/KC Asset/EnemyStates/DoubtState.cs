using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubtState : FSMState
{
    public GameObject Player;

  

    void Awake()
    {
        stateID = StateID.Doubt;

        AddTransition(Transition.IntoIdeal, StateID.Ideal);

        AddTransition(Transition.IntoChasing, StateID.Chasing);

        AddTransition(Transition.IntoDead, StateID.Dead);

        Player = GameObject.Find("Player");

    }

    private void Start()
    {
      
    }


    public override void DoBeforeEntering()
    {
        //Debug.Log("In Doubt: Ehmmm£¿What I just saw?");
        transform.parent.LookAt(transform.parent.position);
    }



    public override void DoBeforeLeaving()
    {

        transform.parent.LookAt(transform.parent.position);

       
    }


    public override void Reason()
    {
        if (GetComponentInParent<EnemyState>().HP <= 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }


        if (GetComponentInParent<Attention>().attentionValue < 10)
        {
            manager.Fsm.PerformTransition(Transition.IntoIdeal);
        }


        if (GetComponentInParent<Attention>().attentionValue >= 50)
        {
            manager.Fsm.PerformTransition(Transition.IntoChasing);
        }


    }

    public override void Act()
    {

        if (GetComponentInParent<Attention>().attentionValue > 10 && GetComponentInParent<EnemyState>().ReturnFromChase==false && Vector3.Distance(transform.position,Player.transform.position)<=GetComponentInParent<FieldOfView>().viewRadius) {
            transform.parent.LookAt(Player.transform.position);
        }

        
    }
}
