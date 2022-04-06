using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubtState : FSMState
{
    public GameObject Player;

    Animator anim;

    void Awake()
    {
        stateID = StateID.Doubt;

        AddTransition(Transition.IntoIdeal, StateID.Ideal);

        AddTransition(Transition.IntoChasing, StateID.Chasing);

        AddTransition(Transition.IntoDead, StateID.Dead);

        if (Player == null) { Player = GameObject.Find("Player"); }

        anim = transform.parent.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
      
    }


    public override void DoBeforeEntering()
    {
        //Debug.Log("In Doubt: Ehmmm£¿What I just saw?");
        transform.parent.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        // Update animator parameter
        anim.SetTrigger("Doubt");
    }



    public override void DoBeforeLeaving()
    {

        transform.parent.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));


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
            transform.parent.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
        }

        
    }
}
