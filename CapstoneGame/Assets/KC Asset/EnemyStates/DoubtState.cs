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
       
        Player = GameObject.Find("Player");

    }

    private void Start()
    {

    }


    public override void DoBeforeEntering()
    {
        Debug.Log("In Doubt: Ehmmm£¿What I just saw?");
    }



    public override void DoBeforeLeaving()
    {

    }


    public override void Reason()
    {
        if (GetComponentInParent<Attention>().attentionValue < 5)
        {
            manager.Fsm.PerformTransition(Transition.IntoIdeal);
        }


        if (GetComponentInParent<Attention>().attentionValue >= 30)
        {
            manager.Fsm.PerformTransition(Transition.IntoChasing);
        }


    }

    public override void Act()
    {
        transform.parent.LookAt(Player.transform.position);
    }
}
