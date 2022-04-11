using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{

    public  bool LastInput;
    public float Timer;


    public bool ReturnFromChase = false;

    public GameObject InitialPositionHolder;

    public bool IsPatrol;

    public GameObject Player;

    [Header("Basic")]
    public float HP;
    public float DisToPlayer;

    Animator anim;
    public ParticleSystem sparks;

    float lastHP, currentHP;
    
    // Used to find the current speed of the enemy
    Vector3 lastPos, currentPos;

    private void Awake()
    {
        if (Player == null) { Player = GameObject.Find("Player"); }
        //Player = GameObject.Find("Player");

        // Create initial position holder for referencing initial position and rotation of enemy
        InitialPositionHolder = new GameObject("InitialPositionHolder");
        InitialPositionHolder = Instantiate(InitialPositionHolder, this.transform.position, Quaternion.identity);
        InitialPositionHolder.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y);

        anim = GetComponentInChildren<Animator>();
        sparks = GetComponentInChildren<ParticleSystem>();
    }
    
    // Update is called once per frame
    void Update()
    {
        DisToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        Timer = Timer - Time.deltaTime;
        if (Timer <= -1) { Timer = -1; }
        LastLureInPut();

        // Main area for updating animation parameters
        anim.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);

        // Code for damage animation (checks HP one time through whole AI loop)
        
        currentHP = HP;

        if (currentHP < lastHP)
        {
            anim.SetTrigger("Hurt");
        }
        lastHP = HP;
    }



    void LastLureInPut()
    {
        //if (Input.GetKey(KeyCode.G))
        //{
        //    Timer = 3;
            
        //}
        

        if (Timer <= 0)
        {
            LastInput = true;
        }
        else { LastInput = false; }
       
    }
}
