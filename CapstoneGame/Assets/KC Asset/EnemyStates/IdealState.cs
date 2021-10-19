using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdealState : FSMState
{
    void Awake()
    {
        stateID = StateID.Ideal;
        AddTransition(Transition.IntoChasing, StateID.Chasing); 
    }





    public override void DoBeforeEntering()
    {
       Debug.Log("I am ready in to the Ideal now");


    }

    public override void Act()
    {
        Debug.Log("NPC In ideal");
        Debug.Log("11111");

    }



    public override void Reason()
    {

        if (Input.GetMouseButtonUp(0)) { manager.Fsm.PerformTransition(Transition.IntoChasing); }
        
    }

    public override void DoBeforeLeaving()
    {
        Debug.Log("Leaving Ideal");
    }
}
