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
    public GameObject SkillScriptFrom;




    float Timer;
    
    bool SkillUsed;

    Animator anim;

    void Awake()
    {
        stateID = StateID.Attack;
        AddTransition(Transition.IntoChasing, StateID.Chasing);
        AddTransition(Transition.IntoDead, StateID.Dead);



        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        if (Player == null) { Player = GameObject.Find("Player"); }
        Timer = SkillCoolDown;
        //skill part
        SkillUsed = false;


        if (transform.parent.gameObject.CompareTag("DevilHead")) {
            EnemyDevilHead = true;
        }

        anim = transform.parent.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (GetComponent<ChaseState>().holdPosition)
        {
            // Make sure player is being looked at (for aiming)
            transform.parent.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
        }
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



        dist = Vector3.Distance(Player.transform.position, transform.parent.position);
        

        //from here for devil head special attack
        if (EnemyDevilHead) {


            if (dist <= FireBallRange && SkillUsed==false) {
                // Update animator parameter
                anim.SetTrigger("RangedAttack");

                // Delayed firing of fireball to reflect animation's charge time
                StartCoroutine(DelayedFireball());
            }
        }

        // Check if charging ranged attack first before trying melee (don't melee if charging ranged)
        if (!GetComponent<ChaseState>().holdPosition)
        {
            //MeleeAttack out range
            if ((!EnemyDevilHead || SkillUsed) && dist > MeleeAttack)
            {
                agent.SetDestination(Player.transform.position);
            }
            else if (dist <= MeleeAttack)
            {
                StartCoroutine(DelayedMeleeAttack());

                // Update animator parameter
                anim.SetTrigger("Attacking");
            }
        }
            
        
        

        //skill timer
        if (SkillUsed)
        { 
            SkillCoolDown -= Time.deltaTime;
            if (SkillCoolDown <= 0)
            {
                SkillScriptFrom.GetComponent<Projectile>().rdyShoot = true;
                resetTimer();
            }
        }


    }


    public void resetTimer() {
        SkillCoolDown = Timer;
        SkillUsed = false;
    
    }

    // In case coroutines are still running when destroyed
    void OnDestroy()
    {
        StopCoroutine(DelayedFireball());
        StopCoroutine(DelayedMeleeAttack());
    }

    IEnumerator DelayedFireball()
    {
        GetComponent<ChaseState>().holdPosition = true;

        yield return new WaitForSeconds(3f);

        GetComponent<ChaseState>().holdPosition = false;

        //active skill
        Debug.Log("Fireeeeee Ballllll~");
        SkillScriptFrom.GetComponent<Projectile>().Shoot();
        SkillScriptFrom.GetComponent<Projectile>().rdyShoot = false;


        //skill used
        SkillUsed = true;
    }

    IEnumerator DelayedMeleeAttack()
    {
        yield return new WaitForSeconds(0.25f);

        // If player is still within melee range
        if (dist <= MeleeAttack)
        {
            if (Player.GetComponent<DamageKnockback>())
            {
                // Damage nearby player character
                Player.GetComponent<DamageKnockback>().ApplyDamage(transform.position, 1, 1);
            }
        }
    }
}
