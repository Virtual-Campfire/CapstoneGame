using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdealState : FSMState
{
    void Awake()
    {
        stateID = StateID.Ideal;
        AddTransition(Transition.IntoDoubt, StateID.Doubt); 
    }


    private void Start()
    {
        
    }


    private void Update()
    {
        if (GetComponentInParent<Attention>().attentionValue == 0) {
            GetComponentInParent<EnemyState>().ReturnFromChase = false;
        }
    }

    public override void DoBeforeEntering()
    {
       Debug.Log("I am ready in to the Ideal now");
      

    }

    public override void Act()
    {
        Debug.Log("NPC In ideal");
        

    }



    public override void Reason()
    {

        if (GetComponentInParent<Attention>().attentionValue>10) { manager.Fsm.PerformTransition(Transition.IntoDoubt); }
        
    }

    public override void DoBeforeLeaving()
    {
        Debug.Log("Leaving Ideal");
    }
}
