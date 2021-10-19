using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public FSMSystem Fsm;



    private void Awake()
    {
        Fsm = new FSMSystem();
    

        FSMState [] states = GetComponentsInChildren<FSMState>();
        foreach (FSMState state in states)
        {
            Fsm.AddState(state, this); //将状态，逐个添加到 状态机中
        }
        IdealState idealState = GetComponentInChildren<IdealState>();
        Fsm.SetCurrentState(idealState);
    }

    private void Update()
    {
        Fsm.UpdateFSM();
    }
}
