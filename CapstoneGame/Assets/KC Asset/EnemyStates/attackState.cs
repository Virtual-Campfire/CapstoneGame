using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class attackState : FSMState
{

   UnityEngine.AI.NavMeshAgent agent;
    public GameObject Player;
    float dist;
   
    //link attack range with the setting range from here
    public float AttackRange;

    public bool EnemyDevilHead;


    [Header("Skill Setting")]
    public float FireBallRange;
    public float SkillCoolDown;
    public float MeleeAttack;




    float Timer;
    
    bool SkillUsed;


    void Awake()
    {
        stateID = StateID.Attack;
        AddTransition(Transition.IntoChasing, StateID.Chasing);
        AddTransition(Transition.IntoDead, StateID.Dead);



        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        Timer = SkillCoolDown;
        //skill part
        SkillUsed = false;


        if (transform.parent.gameObject.CompareTag("DevilHead")) {
            EnemyDevilHead = true;
        }


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
        

        //from here for devil head special attack
        if (EnemyDevilHead) {


            if (dist <= FireBallRange && SkillUsed==false) {
                //active skill
                Debug.Log("Fireeeeee Ballllll~");


                //skill used
                SkillUsed = true;

            }


            //MeleeAttack out range
            if (SkillUsed && dist > MeleeAttack)
            {

                agent.SetDestination(Player.transform.position);
                

            }
            else if (dist <= MeleeAttack) {
                //Melee attack put here
                Debug.Log("if you want put melee attack caculation, in here and delet this line after finish");
            }
                
                    
                    
                    
         }




        //skill timer
        if (SkillUsed)
        { 
            SkillCoolDown -= Time.deltaTime;
            if (SkillCoolDown <= 0)
            {
                resetTimer();
            }
        }






        // Damage nearby player character
        Player.GetComponent<DamageKnockback>().ApplyDamage(transform.position, 1, 1);
    }


    public void resetTimer() {
        SkillCoolDown = Timer;
        SkillUsed = false;
    
    }
}
