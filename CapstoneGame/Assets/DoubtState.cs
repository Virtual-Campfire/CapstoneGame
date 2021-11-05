using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubtState : FSMState
{
    public GameObject Player;
    public GameObject MyHolder;
    void Awake()
    {
        stateID = StateID.Doubt;
        AddTransition(Transition.IntoChasing, StateID.Chasing);
        AddTransition(Transition.IntoIdeal, StateID.Ideal);
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
        //if (GetComponentInParent<Attention>().attentionValue>=30)
        //{
        //    manager.Fsm.PerformTransition(Transition.IntoChasing);
        //}

        if (GetComponentInParent<Attention>().attentionValue == 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoIdeal);
        }
    }

    public override void Act()
    {
        
        transform.parent.LookAt(Player.transform.position);
        Debug.Log("Looking works");

    }
}
