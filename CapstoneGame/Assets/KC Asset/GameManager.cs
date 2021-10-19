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
            Fsm.AddState(state, this); //��״̬�������ӵ� ״̬����
        }
        IdealState idealState = GetComponentInChildren<IdealState>();
        Fsm.SetCurrentState(idealState);
    }

    private void Update()
    {
        Fsm.UpdateFSM();
    }
}
