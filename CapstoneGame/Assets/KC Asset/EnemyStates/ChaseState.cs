using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{


    void Awake()
    {
        stateID = StateID.Chasing;
        AddTransition(Transition.IntoIdeal, StateID.Ideal);
    }

    private void Start()
    {

    }


    public override void DoBeforeEntering()
    {
        Debug.Log("I am ready in to the chasing now");
    }



    public override void DoBeforeLeaving()
    {
        Debug.Log("Leaving chase");
    }


    public override void Reason()
    {
        if (Input.GetMouseButtonUp(0))
        {
            manager.Fsm.PerformTransition(Transition.IntoIdeal);
        }
    }

    public override void Act()
    {

            Debug.Log("22222");

    }




}
