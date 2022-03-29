using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LureState : FSMState
{
    GameObject Player;

    public float LureRange, VisionRange, MaxVisionRange;

    UnityEngine.AI.NavMeshAgent agent;




    public float DelayBeforeDoing;

    private void Awake()
    {
        stateID = StateID.Lure;

        AddTransition(Transition.IntoDoubt, StateID.Doubt);

        AddTransition(Transition.IntoChasing, StateID.Chasing);

        AddTransition(Transition.IntoDead, StateID.Dead);

        AddTransition(Transition.IntoIdeal, StateID.Ideal);
        AddTransition(Transition.IntoLure, StateID.Lure);



        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");

        UpdateRange();

        // Get default view range
        MaxVisionRange = GetComponentInParent<FieldOfView>().viewRadius;
        VisionRange = MaxVisionRange;
    }

    void FixedUpdate()
    {
        // Update value if changed (in case lure range were to change)
        UpdateRange();
    }

    public override void Act()
    {
        
        float dis= Vector3.Distance(transform.position, Player.transform.position);

        if (Player.GetComponent<PlayerController>())
        {
            if (LureRange > dis && Player.GetComponent<PlayerController>().PlayLure == true)
            {
                print("Lure method A received.");
                //agent.SetDestination(Player.transform.position);

            }
        }
        // Alternative variable check
        else if (Player.GetComponent<CharacterController_Player>())
        {
            if (LureRange > dis && Player.GetComponent<CharacterController_Player>().playingSiren == true)
            {
                print("Lure method B received.");
                //agent.SetDestination(Player.transform.position);

                // Reduce vision cone radius when instrument is playing and not alerted to the player
                VisionRange -= Time.deltaTime * MaxVisionRange;

                // Cap vision radius at 0 units
                if (VisionRange <= 0)
                {
                    VisionRange = 0;

                    // If any animations for sleeping are available, trigger them here
                }
            }
        }
        else
        {
            agent.SetDestination(transform.position);

            // Regenerate vision radius
            VisionRange = MaxVisionRange;

            // Cap vision radius at default
            if (VisionRange > MaxVisionRange)
            {
                GetComponentInParent<FieldOfView>().viewRadius = VisionRange;
            }
        }
        
        // Update vision cone radius
        GetComponentInParent<FieldOfView>().viewRadius = VisionRange;
    }

    public override void DoBeforeLeaving()
    {
        // Return vision radius to default (for example, if woken up)
        VisionRange = MaxVisionRange;

        // Update vision cone radius
        GetComponentInParent<FieldOfView>().viewRadius = VisionRange;
    }



    public override void Reason()
    {
        if (GetComponentInParent<EnemyState>().HP <= 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoDead);

        }

        if (GetComponentInParent<Attention>().attentionValue > 10)
        {
            manager.Fsm.PerformTransition(Transition.IntoDoubt);
        }

        if (GetComponentInParent<Attention>().attentionValue >= 50)
        {
            manager.Fsm.PerformTransition(Transition.IntoChasing);
        }

        if (Player.GetComponent<PlayerController>())
        {
            if (GetComponentInParent<Attention>().attentionValue <= 10 && Player.GetComponent<PlayerController>().PlayLure == false && GetComponentInParent<EnemyState>().Timer < 0)
            {

                if (GetComponentInParent<EnemyState>().LastInput == true) { Invoke("Return", DelayBeforeDoing); }


            }

            if (Player.GetComponent<PlayerController>().PlayLure == true && GetComponentInParent<EnemyState>().DisToPlayer <= Player.GetComponent<PlayerController>().Range)
            {
                manager.Fsm.PerformTransition(Transition.IntoLure);
            }
        }

        // Alternative variable check
        if (Player.GetComponent<CharacterController_Player>())
        {
            if (GetComponentInParent<Attention>().attentionValue <= 10 && Player.GetComponent<CharacterController_Player>().playingSiren == false && GetComponentInParent<EnemyState>().Timer < 0)
            {

                if (GetComponentInParent<EnemyState>().LastInput == true) { Invoke("Return", DelayBeforeDoing); }

                // Alternative variable check
                if (Player.GetComponent<CharacterController_Player>().playingSiren == true && GetComponentInParent<EnemyState>().DisToPlayer <= LureRange)
                {
                    manager.Fsm.PerformTransition(Transition.IntoLure);
                }
            }
        }
    }


    void Return() {
        // Only exit out to idle state if not asleep
        if (VisionRange != 0)
        {
            manager.Fsm.PerformTransition(Transition.IntoIdeal);
        }
    }

   

    void UpdateRange()
    {
        // Conditional statement to avoid null refs when exclusively using alternative lure calls
        if (Player.GetComponent<PlayerController>())
        {
            LureRange = Player.GetComponent<PlayerController>().Range;
        }
        else if (Player.GetComponent<CharacterController_Player>())
        {
            LureRange = Player.GetComponent<CharacterController_Player>().AOEEffectRadius;
        }
    }

}
