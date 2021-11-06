using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackState : FSMState
{
    public GameObject Player;
    float dist;


    //link attack range with the setting range from here
    public float AttackRange;

    void Awake()
    {
        stateID = StateID.Attack;
        AddTransition(Transition.IntoChasing, StateID.Chasing);
        Player = GameObject.Find("Player");


    }

    private void Start()
    {

    }

    private void Update()
    {
        dist = Vector3.Distance(Player.transform.position, transform.position);
    }


    public override void DoBeforeEntering()
    {
      
    }



    public override void DoBeforeLeaving()
    {

    }


    public override void Reason()
    {
        if (dist>AttackRange)
        {
            manager.Fsm.PerformTransition(Transition.IntoChasing);
        }
    }

    public override void Act()
    {

        Debug.Log("Detroit Smash~~!!");
    }
}
