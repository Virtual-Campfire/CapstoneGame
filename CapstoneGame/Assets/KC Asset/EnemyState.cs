using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{

    public  bool LastInput;
    public float Timer;


    public bool ReturnFromChase = false;

    public GameObject InitialPositionHolder;

    public bool IsPatrol;

    GameObject Player;

    [Header("Basic")]
    public float HP;
    public float DisToPlayer;



    private void Awake()
    {
        Player = GameObject.Find("Player");

        // Create initial position holder for referencing initial position and rotation of enemy
        InitialPositionHolder = new GameObject("InitialPositionHolder");
        InitialPositionHolder = Instantiate(InitialPositionHolder, this.transform.position, Quaternion.identity);
        InitialPositionHolder.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y);
    }
    
    // Update is called once per frame
    void Update()
    {
        DisToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        Timer = Timer - Time.deltaTime;
        if (Timer <= -1) { Timer = -1; }
        LastLureInPut();

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
