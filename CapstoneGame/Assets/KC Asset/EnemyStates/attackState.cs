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
        AddTransition(Transition.IntoDead, StateID.Dead);

        Player = GameObject.Find("Player");


    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }


    public override void DoBeforeEntering()
    {
      
    }



    public override void DoBeforeLeaving()
    {

    }


    public override void Reason()
    {

        if (GetComponentInParent<EnemyState>().HP <= 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }


        if (dist>AttackRange)
        {
            manager.Fsm.PerformTransition(Transition.IntoChasing);
        }
    }

    public override void Act()
    {

        dist = Vector3.Distance(Player.transform.position, transform.position);

        Debug.Log("Detroit Smash~~!!");

        // Damage nearby player character
        Player.GetComponent<DamageKnockback>().ApplyDamage(transform.position, 1, 1);
    }
}
