using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadState : FSMState
{

    public GameObject Pa;
    public Material Ma;
    public Material LightIn;
    
    [HideInInspector]
    public float ControlCaulate;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        //Ma = Pa.GetComponent<Material>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Awake()
    {
        stateID = StateID.Dead;
        Pa = transform.parent.gameObject;

    }

    public override void Act()
    {
        ControlCaulate = ControlCaulate + Time.deltaTime * Speed;
        Ma.SetFloat("_Control", ControlCaulate);
        LightIn.SetFloat("_control", ControlCaulate);

        if (ControlCaulate >= 1) {

            Destroy(Pa);
        }
       

       



    }


    public override void DoBeforeEntering()
    {
        ControlCaulate = 0;

    }
    public override void DoBeforeLeaving()
    {
        ControlCaulate = 0;
    }


}
